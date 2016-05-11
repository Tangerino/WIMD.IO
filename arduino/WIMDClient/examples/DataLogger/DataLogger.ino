#include <WIMD.h>
#include <SPI.h>
#include <Ethernet.h>

#define SS_SD_CARD   4
#define SS_ETHERNET 10

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };

EthernetClient client;
WIMDClient wimdclient(client,"f2f69c43-11d3-11e6-8e61-04017fd5d401");


void addRecord(){
  int value;
  enableEthernet();
  
  char val1[4],val2[4],val3[4],val4[4];
   
  Serial.println(F("Creating Sensor"));
  WIMDSensor sensor;

  sensor.build("88888","Current","Ampere","amp");
  if(wimdclient.createSensor(sensor))
    Serial.println(F("Sensor created"));

  sensor.build("99999","Voltage","Volt","v");
  if(wimdclient.createSensor(sensor))
    Serial.println(F("Another sensor created"));

 
  WIMDSensorValue series1[]={
       WIMDSensorValue("",itoa(random(10,100),val1,10)), //date,value
       WIMDSensorValue("",itoa(random(10,100),val2,10)), //date,value
       WIMDSensorValue("",itoa(random(10,100),val2,10)), //date,value
  };

  WIMDSensorValue series2[]={
         WIMDSensorValue("",itoa(random(10,100),val3,10)), //date,value
         WIMDSensorValue("",itoa(random(10,100),val4,10)), //date,value
  };

  WIMDFeed feeds[]={
    WIMDFeed("88888",series1 , 3), //remoteid, series, count of series
    WIMDFeed("99999",series2 , 2)
  };

     
  WIMDRequest request(feeds,2); // feeds, numberof sensors
  
  //wimdclient.enableDebug(false);
  //now deleteSensor(remoteId)
  if(wimdclient.put(request)){
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
  Ethernet.begin(mac);
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