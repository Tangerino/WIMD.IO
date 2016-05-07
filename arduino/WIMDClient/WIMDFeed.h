#ifndef WIMDFeed_h
/*
  WIMDSensorValue.h - Arduino WIMDClient Library
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
*/
#define WIMDFeed_h
#include "WIMDSensorValue.h"


class WIMDFeed: public Printable
{
	public:
		WIMDFeed(const char* remoteId,WIMDSensorValue* sensorValues, int sensorsCount);
		virtual size_t printTo(Print&) const;
		const char* id() { return _remoteId; };
		int size() { return _sensorsCount; };
		WIMDSensorValue& operator[] (unsigned i) { return _sensorValues[i]; };
		int printToBuff(char* buff);

		
	//protected:
		const char* _remoteId;
		int _sensorsCount;
		WIMDSensorValue* _sensorValues;
		
				
};

#endif