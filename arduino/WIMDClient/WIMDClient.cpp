/*
  WIMDClient.c - Arduino WIMDClient Library 
  This class handles communication to WIMD server using ethernet client
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
*/

#include "Arduino.h"
#include "WIMDClient.h"

/*
*  The constructor WIMDClient create instance of WIMD Client
*  @param EthernetClient, devKey
*  @return void
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
WIMDClient::WIMDClient(EthernetClient& client, const char* devKey): _client(client),_devKey(devKey){
	Serial.begin(9600);
}

/*
*  Creates sensor to WIMD Server, communicates via EthernetClient
*  @param WIMDSensor 
*  @return true if success, false if failed or error
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
int WIMDClient::createSensor(WIMDSensor& sensor){
	char buff[200];
	int len=sensor.printToBuff(buff);
	char resp[]="remoteid";
	
	if (_client.connect("wimd.io", 80)) {
		if(_debug)
			Serial.println(F("connected"));
		
		// Make a HTTP request:
		_client.println("POST /v2/sensor HTTP/1.1");
		_client.println("Host: wimd.io");
		_client.println("Connection: keep-alive");
		_client.println("Content-type: application/json");
		_client.print("Content-length: ");
		_client.println(len);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		_client.println(buff);
		_client.println();
	
		return waitForResponse(resp);
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
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
int WIMDClient::deleteSensor(const char* remoteId){
	char buff[200];
	char resp[]="OK";
	int sum=0;
	
	if (_client.connect("wimd.io", 80)) {
		if(_debug)
			Serial.println(F("connected"));
	
		// Make a HTTP request:
		_client.print("DELETE /v2/sensor/");
		_client.print(remoteId);
		_client.println(" HTTP/1.1");
		_client.println("Host: wimd.io");
		_client.println("Connection: keep-alive");
		_client.print("Content-length: ");
		_client.println(0);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		
		
		return waitForResponse(resp);
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
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
int WIMDClient::updateSensor(const char* remoteId){
	char buff[200];
	char resp[]="OK";
	
	if (_client.connect("wimd.io", 80)) {
		if(_debug)
			Serial.println(F("connected"));
	
		// Make a HTTP request:
		_client.print("PUT /v2/sensor/");
		_client.print(remoteId);
		_client.println(" HTTP/1.1");
		_client.println("Host: wimd.io");
		_client.println("Connection: keep-alive");
		_client.print("Content-length: ");
		_client.println(0);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		
		
		return waitForResponse(resp);
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
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
int WIMDClient::getDataStream(char* buf, WIMDFeed& aFeed)
{
    strcpy(buf+strlen(buf),"{");
    strcpy(buf+strlen(buf),"\"id\":\"");
    strcpy(buf+strlen(buf),aFeed.id());
    strcpy(buf+strlen(buf),"\",");
    strcpy(buf+strlen(buf),"\"values\" : [");
    for(int i = 0; i < aFeed._sensorsCount;i++){
        aFeed._sensorValues[i].printToBuff(buf+strlen(buf));
        if (i == aFeed._sensorsCount-1){
            //to do something?
        }else{
          strcpy(buf+strlen(buf),",");
        }    
    }
    strcpy(buf+strlen(buf),"]}");
    return (strlen(buf));
}


/*
*  Uploads data to WIMD server
*  @param WIMDFeed
*  @return boolean, true on success
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
int WIMDClient::put(WIMDFeed& aFeed)
{
    
    char request[BUFF_LEN];
    char* data = request+BUFF_LEN/2;
    memset(request,'\0',sizeof(request));
    char resp[]="OK";
    

    int dataLen = getDataStream(data,aFeed);
       
    if (_client.connect("wimd.io", 80)) {
    	if(_debug)
			Serial.println(F("connected"));
	
		// Make a HTTP request:
		_client.print("POST /v2/sensor/data");
		_client.println(" HTTP/1.1");
		_client.println("Host: wimd.io");
		_client.println("Connection: keep-alive");
		_client.print("Content-length: ");
		_client.println(dataLen);
		_client.print("devkey: ");
		_client.println(_devKey);
		_client.println();
		_client.println(data);
		
		return waitForResponse(resp);
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
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
bool WIMDClient::waitForResponse(const char* resp)
{
	unsigned long timerStart,timerEnd;
    timerStart = millis();
    timerEnd = DEFAULT_WAIT_RESP_TIMEOUT + timerStart;
    int sum=0;
    while(1) {

    	if (_client.available()) {
			char c = _client.read();
			if(_debug)
				Serial.print(c);

			sum = (c==resp[sum]) ? sum+1 : 0;
            if(sum == strlen(resp)){
            	_client.stop();
            	return true;
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
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
char* WIMDClient::getCurrentDateTime(){
	
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
*  Author Sagar Devkota<sagarda7@yahoo.com> 
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




