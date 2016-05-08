#include <WIMD.h>
#include <SPI.h>
#include <Ethernet.h>
#include<SD.h>

#define SS_SD_CARD   4
#define SS_ETHERNET 10

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
byte ip[] = { 192, 168, 0, 177 };

EthernetClient client;
WIMDClient wimdclient(client,"f2f69c43-11d3-11e6-8e61-04017fd5d401");


void addRecord(){

  
  int value;

  enableEthernet();
  value=random(10,100);
  char date[20];
  strcpy(date,wimdclient.getCurrentDateTime());

  enableSD();
  File myFile = SD.open("log.txt", FILE_WRITE);
  if (myFile) {
    
    Serial.println(F("writing.."));
    //Serial.println(date);
   
    myFile.print(date);
    myFile.print(",");
    myFile.println(value);
    // close the file:
    myFile.close();

    // now upload
    enableEthernet();
    char val[4];
  
    WIMDSensorValue sensorValues[] = {
      WIMDSensorValue("",itoa(value,val,10))
    };
  
    WIMDFeed feed("56789", sensorValues, 1); // remoteid, sensorvalues, number of values
    
    //wimdclient.enableDebug(false);
    //now deleteSensor(remoteId)
    if(wimdclient.put(feed)){
      Serial.println(F("Data Added"));
    }
    else
    {
      Serial.println(F("Data Not Added"));
    }
    
  } else {
    // if the file didn't open, print an error:
    Serial.println(F("error opening file"));
  }
}


void setup()
{
  randomSeed(A0); 
  Serial.begin(9600);
  pinMode(SS_SD_CARD, OUTPUT);
  pinMode(SS_ETHERNET, OUTPUT);
  digitalWrite(SS_SD_CARD, HIGH);  // SD Card not active
  digitalWrite(SS_ETHERNET, HIGH); // Ethernet not active
    
  enableEthernet();
  Ethernet.begin(mac, ip);
  wimdclient.enableDebug(true);
  enableSD();
  if(SD.begin(SS_SD_CARD)){
    Serial.println("SD card OK");
  }
  else
  {
    Serial.println("SD Card Error");
  }
 
  

  
  
  
 
}

void loop()
{
  addRecord();
  delay(60000);
}




void enableSD() {
    
    // ...
    digitalWrite(SS_SD_CARD, LOW);  // SD Card ACTIVE
    // code that sends to the sd card slave device over SPI
    // using SPI.transfer() etc.
    digitalWrite(SS_ETHERNET, HIGH); // SD Card not active
    // ...
    
}

void enableEthernet() {
    // ...
    digitalWrite(SS_ETHERNET, LOW);  // Ethernet ACTIVE
    // code that sends to the ethernet slave device over SPI
    // using SPI.transfer() etc.
    digitalWrite(SS_SD_CARD, HIGH); // Ethernet not active
    // ...
}








