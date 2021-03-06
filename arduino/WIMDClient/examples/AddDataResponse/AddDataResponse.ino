#include <WIMD.h>
#include <SPI.h>
#include <Ethernet.h>
#define DEV_KEY "f2f69c43-11d3-11e6-8e61-04017fd5d401"
#define SS_SD_CARD   4
#define SS_ETHERNET 10

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
byte ip[] = { 192, 168, 0, 177 };

//lets create client
EthernetClient client;
//Create WIMD client 
WIMDClient wimdclient(client,DEV_KEY);

void setup()
{
  pinMode(SS_SD_CARD, OUTPUT);
  pinMode(SS_ETHERNET, OUTPUT);
  digitalWrite(SS_SD_CARD, HIGH);  // SD Card not active
  digitalWrite(SS_ETHERNET, HIGH); // Ethernet not active
  
  Serial.begin(9600);
  
  enableEthernet();
  Ethernet.begin(mac, ip);
  char buff[200];
 
  WIMDSensorValue series1[]={
           WIMDSensorValue("2016-02-16 14:30:50","144.5"),
           WIMDSensorValue("2016-02-17 23:55:50","96.5")
      };

  WIMDSensorValue series2[]={
           WIMDSensorValue("2016-02-15 09:30:50","100.8"),
           WIMDSensorValue("2016-02-16 11:55:50","29.4")
      };
      
  WIMDFeed feeds[]={
      WIMDFeed("56789",series1 , 2),
      WIMDFeed("223344", series2, 2)
  };

  WIMDRequest request(feeds,2);



  //wimdclient.enableDebug(true);
  //now deleteSensor(remoteId)
  if(wimdclient.put(request,buff)){
    Serial.println("Data Added");
    Serial.println(buff); //response
    
  }
  else
  {
    Serial.println("Data Not Added");
  }

  
}

void loop()
{
  
}

// it enables ethernet spi disabling SD spi if SD card is inserted
void enableEthernet() {
    // ...
    digitalWrite(SS_ETHERNET, LOW);  // Ethernet ACTIVE
    // code that sends to the ethernet slave device over SPI
    // using SPI.transfer() etc.
    digitalWrite(SS_ETHERNET, HIGH); // Ethernet not active
    // ...
}