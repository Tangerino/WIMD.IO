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
#ifndef WIMDClient_h
/*
  WIMDClient.h - Arduino WIMDClient Library
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