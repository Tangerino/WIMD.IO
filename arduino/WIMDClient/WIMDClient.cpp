	/*
	* The MIT License (MIT)
	* 
	* Copyright (c) 2016 Carlos Tangerino
	* 
	* Permission is hereby granted, free of charge, to any person obtaining a copy
	* of this software and associated documentation files (the "Software"), to deal
	* in the Software without restriction, including without limitation the rights
	* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	* copies of the Software, and to permit persons to whom the Software is
	* furnished to do so, subject to the following conditions:
	* 
	* The above copyright notice and this permission notice shall be included in all
	* copies or substantial portions of the Software.
	* 
	* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	* SOFTWARE.
	*/ 

/*
  WIMDClient.c - Arduino WIMDClient Library 
  This class handles communication to WIMD server using ethernet client
*/

#include "Arduino.h"
#include "WIMDClient.h"

/*
*  The constructor WIMDClient create instance of WIMD Client
*  @param EthernetClient, devKey
*  @return void
*  @since 1.0
*/
WIMDClient::WIMDClient(EthernetClient& client, const char* devKey): _client(client),_devKey(devKey){
	Serial.begin(9600);
}

/*
*  Creates sensor to WIMD Server, communicates via EthernetClient
*  @param WIMDSensor 
*  @return true if success, false if failed or error
*  @since 1.0
*/
int WIMDClient::createSensor(WIMDSensor& sensor){
	char buff[120];
	int len=sensor.printToBuff(buff);
	//char resp[]="remoteid";
	
	if (_client.connect(BASE_URI, 80)) {
		if(_debug)
			Serial.println(F("connected"));
		
		// Make a HTTP request:
		_client.println("POST /v2/sensor HTTP/1.1");
		_client.print("Host: ");
		_client.println(BASE_URI);
		_client.println("Connection: keep-alive");
		_client.println("Content-type: application/json");
		_client.print("Content-length: ");
		_client.println(len);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		_client.println(buff);
		_client.println();
	
		return waitForResponse();
	  }
	  else {
		return false;
	  }
	  
	  
}

/*
*  Creates sensor to WIMD Server, communicates via EthernetClient
*  @param WIMDSensor, buffer 
*  @return true if success, false if failed or error
*  @since 1.0
*/
int WIMDClient::createSensor(WIMDSensor& sensor, char* buff){
	char buffer[120];
	int len=sensor.printToBuff(buffer);
	//char resp[]="remoteid";
	
	if (_client.connect(BASE_URI, 80)) {
		if(_debug)
			Serial.println(F("connected"));
		
		// Make a HTTP request:
		_client.println("POST /v2/sensor HTTP/1.1");
		_client.print("Host: ");
		_client.println(BASE_URI);
		_client.println("Connection: keep-alive");
		_client.println("Content-type: application/json");
		_client.print("Content-length: ");
		_client.println(len);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		_client.println(buffer);
		_client.println();
	
		return waitForResponseWithBuff(buff);
	  }
	  else {
		return false;
	  }
	  
	  
}


/*
*  Deletes sensor in WIMD server
*  @param const char* remoteId , remote id of sensor
*  @return true or false
*  @since 1.0
*/
int WIMDClient::deleteSensor(const char* remoteId){
	char buff[200];
	//char resp[]="OK";
	int sum=0;
	
	if (_client.connect(BASE_URI, 80)) {
		if(_debug)
			Serial.println(F("connected"));
	
		// Make a HTTP request:
		_client.print("DELETE /v2/sensor/");
		_client.print(remoteId);
		_client.println(" HTTP/1.1");
		_client.print("Host: ");
		_client.println(BASE_URI);
		_client.println("Connection: keep-alive");
		_client.print("Content-length: ");
		_client.println(0);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		
		return waitForResponse();
	  }
	  else {
		return false;
	  }
}


/*
*  Updates sensor in WIMD server
*  @param const char* remoteId , remote id of sensor
*  @return true or false
*  @since 1.0
*/
int WIMDClient::updateSensor(WIMDSensor& sensor){
	char buff[120];
	int len=sensor.printToBuff(buff);
	
	if (_client.connect(BASE_URI, 80)) {
		if(_debug)
			Serial.println(F("connected"));
	
		// Make a HTTP request:
		_client.print("PUT /v2/sensor/");
		_client.print(sensor.getId());
		_client.println(" HTTP/1.1");
		_client.print("Host: ");
		_client.println(BASE_URI);
		_client.println("Connection: keep-alive");
		_client.print("Content-length: ");
		_client.println(len);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		_client.println(buff);
		_client.println();
		
		return waitForResponse();
	  }
	  else {
		return false;
	  }
}

/*
*  Gets WIDM sensor values with message body in json format which is ready to upload 
*  @param const char* , WIMDFeed
*  @return integer , length of message
*  @since 1.0
*/
int WIMDClient::getDataStream(char* buf, WIMDRequest& aRequest)
{
    strcpy(buf+strlen(buf),"[");
    for(int x=0; x<aRequest._feedsCount; x++){
	    strcpy(buf+strlen(buf),"{\"id\":\"");
	    strcpy(buf+strlen(buf),aRequest._feeds[x].id());
	    strcpy(buf+strlen(buf),"\",");
	    strcpy(buf+strlen(buf),"\"values\" : [");
	    for(int i = 0; i < aRequest._feeds[x]._sensorsCount;i++){
	        aRequest._feeds[x]._sensorValues[i].printToBuff(buf+strlen(buf));
	        if (i == aRequest._feeds[x]._sensorsCount-1){
	            //to do something?
	        }else{
	          strcpy(buf+strlen(buf),",");
	        }    
	    }
	    strcpy(buf+strlen(buf),"]");
	    strcpy(buf+strlen(buf),"}");
	    if (x == aRequest._feedsCount-1){
            //to do something?
        }else{
          strcpy(buf+strlen(buf),",");
        }   
	}
    strcpy(buf+strlen(buf),"]");
    return (strlen(buf));
}


/*
*  Uploads data to WIMD server
*  @param WIMDRequest
*  @return boolean, true on success
*  @since 1.0
*/
int WIMDClient::put(WIMDRequest& aRequest)
{
    
    char request[BUFF_LEN];
    char* data = request+BUFF_LEN/2;
    memset(request,'\0',sizeof(request));

    int dataLen = getDataStream(data,aRequest);
       
    if (_client.connect(BASE_URI, 80)) {
    	if(_debug)
			Serial.println(F("connected"));
	
		// Make a HTTP request:
		_client.print("POST /v2/sensor/data");
		_client.println(" HTTP/1.1");
		_client.print("Host: ");
		_client.println(BASE_URI);
		_client.println("Connection: keep-alive");
		_client.print("Content-length: ");
		_client.println(dataLen);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		_client.println(data);
		
		return waitForResponse();
	}
	else
	{
		return false;
	}		
}



/*
*  Uploads data to WIMD server and gets message body
*  @param WIMDRequest
*  @return boolean, true on success
*  @since 1.0
*/
int WIMDClient::put(WIMDRequest& aRequest, char* buff) {
    char request[BUFF_LEN];
    char* data = request+BUFF_LEN/2;
    memset(request,'\0',sizeof(request));
    //char resp[]="OK";
    

    int dataLen = getDataStream(data,aRequest);
    //Serial.println(data);
       
    if (_client.connect(BASE_URI, 80)) {
    	if(_debug)
			Serial.println(F("connected"));
	
		// Make a HTTP request:
		_client.print("POST /v2/sensor/data");
		_client.println(" HTTP/1.1");
		_client.print("Host: ");
		_client.println(BASE_URI);
		_client.println("Connection: keep-alive");
		_client.print("Content-length: ");
		_client.println(dataLen);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		_client.println(data);
		
		return waitForResponseWithBuff(buff);
	}
	else
	{
		return false;
	}
}


/*
*  Waits for response to come till given timeout
*  @param const char*, value to be searched in response to think valid response
*  @return boolean, true on success
*  @since 1.0
*/
bool WIMDClient::waitForResponse()
{
	unsigned long timerStart,timerEnd;
    timerStart = millis();
    timerEnd = DEFAULT_WAIT_RESP_TIMEOUT + timerStart;
    uint8_t sum=0;
    const char resp[]="HTTP/1.1 20"; //expect 2x response first

    while(1) {

    	if (_client.available()) {
			char c = _client.read();
			if(_debug)
				Serial.print(c);

			sum = (c==resp[sum]) ? sum+1 : 0;
            if(sum == strlen(resp)){
            	c = _client.read();
            	if(c=='1' || c=='0'){  // if rexponse is 200 or 201
            		_client.stop();
            		if(_debug)
						Serial.print(c);

            		return true;
            	}
            } 
		}
		
        
        if(millis() > timerEnd) {
        	_client.stop();
            return false;
        }
    }
    _client.stop();
    return false;
}


/*
*  Waits for response to come till given timeout
*  @param const char*, value to be searched in response to think valid response
*  @return boolean, true on success
*  @since 1.0
*/
bool WIMDClient::waitForResponseWithBuff(char* buff)
{
	unsigned long timerStart,timerEnd;
    timerStart = millis();
    timerEnd = DEFAULT_WAIT_RESP_TIMEOUT + timerStart;
    uint8_t sum=0;
    const char resp[]="HTTP/1.1 20"; //expect 2x response first
    bool success=false, read=false;
    const char seek[]="\r\n\r\n";
    int i=0;

    while(1) {

    	if (_client.available()) {
			char c = _client.read();
			if(_debug)
				Serial.print(c);

			if(success==false){
				sum = (c==resp[sum]) ? sum+1 : 0;
	            if(sum == strlen(resp)){
	            	c = _client.read();
	            	if(c=='1' || c=='0'){  // if rexponse is 200 or 201
	            		sum=0;
	            		success=true;
	            	}
	            }
	        }

            if(success==true) {
            	 sum = (c==seek[sum]) ? sum+1 : 0;
            	 if(sum==strlen(seek)){
            	 	read=true;
            	 	continue;
            	 }

            	 if(read){
            	 	buff[i]=c;
            	 	i++;

            	 	if(c=='}'){

            	 		_client.stop();
            	 		buff[i]='\0';
            	 		return true;
            	 	}
            	 }


            }
		}
        if(millis() > timerEnd) {
        	_client.stop();
            return false;
        }
    }
    _client.stop();
    return false;
}


/*
*  Get current date time from time api
*  @param null 
*  @return char*
*  @since 1.0
*/
const char* WIMDClient::getCurrentDateTime(){
	
	if (_client.connect("www.timeapi.org", 80)) {
		if(_debug)
			Serial.println(F("connected"));
		
		// Make a HTTP request:
		_client.println("GET /utc/now HTTP/1.1");
		_client.println("Host: www.timeapi.org");
		_client.println("Connection: keep-alive");
		_client.println("User-Agent: Mozilla/5.0");
		_client.println();
		
		unsigned long timerStart,timerEnd;
	    timerStart = millis();
	    timerEnd = DEFAULT_WAIT_RESP_TIMEOUT + timerStart;
	    int sum=0;
	    const char resp[]="vegur";
	    char buff[30];
	    bool found=false;
	    uint8_t index=0;
	    while(1) {

	    	if (_client.available()) {
				char c = _client.read();
				if(_debug)
					Serial.print(c);

				sum = (c==resp[sum]) ? sum+1 : 0;
	            if(sum == strlen(resp)){
	            	found=true;
	            	c=_client.read();
	            } 

	            if(found){
	            	if(c=='+'){
	            		_client.stop();
	            		buff[index]='\0';
	            		_filter(buff);
	            		return buff;
	            	}
	            	buff[index]=c;
	            	index++;
	            }

			}
			
	        
	        if(millis() > timerEnd) {
	        	_client.stop();
	            return "false";
	        }
	    }
	  }
	  else {
		return "false";
	  }
	  
}

/*
*  Filter extra characters in date
*  @param char* buff 
*  @return void
*  @since 1.0
*/
void WIMDClient::_filter(char* buff){
	char i,j=0;
	char newbuff[30];
	for(i=0; i<strlen(buff); i++){
		if(buff[i]!='\n' && buff[i]!='\r'){
			if(buff[i]=='T') buff[i]=32;
			newbuff[j]=buff[i];
			j++;
		}
	}
	strcpy(buff,newbuff);
}




