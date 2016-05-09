# WIMDClient Library For Arduino
WIMDClient library which is used to connect wimd.io(Where is my data) server using arduino

# Install

Clone (or download and unzip) the repository to `/libraries`
which can be found in your arduino installation. currently this library supports remote sensor APIs

# Supported Boards

WIMD Client currently supports Ethernet Shield and All arduino boards (UNO, MINI, MEGA etc).

# Usage

### Include

```c++
#include <WIMD.h>
```

### WIMDClient wimdclient(EthernetClient,const char*); 

Constructor to create an WIMDClient object which takes two parameters
1. EthernetClient object  2. Device Key

```c++
	...
	EthernetClient client;
	WIMDClient wimdclient(client,"xxxxxx-11d3-11e6-8e61-xxxxxxxxxxxx");

	void setup(){
		 ....
		 
	}
```

### Create sensor WIMDClient::createSensor(WIMDSensor sensor)

Creating sensor needs WIMDSensor object that is passed to instance WIMDClient 


```c++
	...
	void setup(){
		  ...
		  WIMDSensor sensor;
		  // now create sensor Object remoteId, Name, UnitName, Unit
		  sensor.build("56789","Current","Ampere","amp");
		  
		  //now pass sensor object to wimd server
		  if(wimdclient.createSensor(sensor)){
		    Serial.println("Sensor Created");
		  }
		  else
		  {
		    Serial.println("Sensor not created");
		  }
		  ...
	}
```

### Delete Sensor WIMDClient::deleteSensor(const char* remoteId)

It deletes the sensor , takes remoteId as parameter

```c++
  void loop(){
  	  ...

  	  if(wimdclient.deleteSensor("56789")){
	    Serial.println("Sensor Deleted");
	  }
	  else
	  {
	    Serial.println("Sensor not deleted");
	  }

	  ...
  }
```


### Update Sensor WIMDClient::updateSensor(WIMDSensor sensor)

It updates the sensor , takes remoteId as parameter

```c++
  void loop(){
  	  ...

  	  WIMDSensor sensor;
	  // now create sensor Object remoteId, Name, UnitName, Unit  to be updated
	  sensor.build("56789","Current","Ampere","amp");
	  
	  //now pass sensor object to wimd server
	  if(wimdclient.updateSensor(sensor)){
	    Serial.println("Sensor Updated");
	  }
	  else
	  {
	    Serial.println("Sensor not updated");
	  }

	  ...
  }
```

### Enable Debug WIMDClient.enableDebug(boolean)

It enables debug so that response can be viewed in Serial Monitor, default false

```c++

  void setup(){
  	...

  	wimdclient.enableDebug(true);  

  	...
  }
```

### Add data to the server WIMDClient::put

It sends time series values to the server
```c++
  void loop(){
  	  ...

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
		 
	  if(wimdclient.put(request)){
	    Serial.println("Data Added");
	  }
	  else
	  {
	    Serial.println("Data Not Added");
	  }

	  ...
  }
```

### Get current datetime(utc) WIMDClient::getCurrentDateTime

It gets the current date/time in utc from server
```c++
  void loop(){
  	  ...

  	  
	    Serial.println(wimdclient.getCurrentDateTime());
	  
	  ...
  }
```


### Full Working example to add static values to remote sensor



```c++
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
	 
	  //create collection of sensor values
	  //WIMDSensorValue(datetime,value)
	  WIMDSensorValue sensorValues[] = {
	    WIMDSensorValue("2016-02-16 14:30:50","144.5"),
	    WIMDSensorValue("2016-02-17 23:55:50","96.5")
	  };

	  WIMDFeed feed("56789", sensorValues, 2); // remoteid, sensorvalues, number of values
	  
	  wimdclient.enableDebug(true);
	  //now deleteSensor(remoteId)
	  if(wimdclient.put(feed)){
	    Serial.println("Data Added");
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


```