#include <WIMD.h>
#include <SPI.h>
#include <Ethernet.h>
#include<SD.h>

#define SS_SD_CARD   4
#define SS_ETHERNET 10

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };

EthernetClient client;
WIMDClient wimdclient(client,"f2f69c43-11d3-11e6-8e61-04017fd5d401");


void addRecord(){
  int value;
  enableEthernet();
  value=random(10,100);
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
 
}

void loop()
{
  addRecord();
  delay(60000);
}


void enableEthernet() {
    // ...
    digitalWrite(SS_ETHERNET, LOW);  // Ethernet ACTIVE
    // code that sends to the ethernet slave device over SPI
    // using SPI.transfer() etc.
    digitalWrite(SS_SD_CARD, HIGH); // SD not active
    // ...
}








