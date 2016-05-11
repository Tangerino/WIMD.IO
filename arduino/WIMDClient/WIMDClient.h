#ifndef WIMDClient_h
/*
  WIMDClient.h - Arduino WIMDClient Library
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
*/
#define WIMDClient_h

#include "Arduino.h"
#define DEFAULT_WAIT_RESP_TIMEOUT 5000
#include <SPI.h>
#include <Ethernet.h>
#include "WIMDSensor.h"
#include "WIMDFeed.h"
#include "WIMDRequest.h"
#define BASE_URI "wimd.io"
  
#define BUFF_LEN    400

class WIMDClient
{
	public:
		WIMDClient(EthernetClient& client, const char* devKey);
		int createSensor(WIMDSensor& sensor);
		int createSensor(WIMDSensor& sensor, char* buff);
		int updateSensor(WIMDSensor& sensor);
		int deleteSensor(const char* remoteId);
		int getDataStream(char* buf, WIMDRequest& aRequest);
		int put(WIMDRequest& aRequest);
		int put(WIMDRequest& aRequest, char* buff);
		void enableDebug(bool debug) {_debug=debug;}
		const char* getCurrentDateTime();

	protected:
		EthernetClient& _client;
		const char* _devKey;
		bool waitForResponse();
		bool waitForResponseWithBuff(char* buff);
		bool _debug;
		void _filter(char* buff);
		
		
	
		
};

#endif