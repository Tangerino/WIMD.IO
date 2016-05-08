#ifndef WIMDSensorValue_h
/*
  WIMDSensorValue.h - Arduino WIMDClient Library
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
*/
#define WIMDSensorValue_h
#include <Stream.h>
#include <Printable.h>

class WIMDSensorValue: public Printable
{
	public:
		WIMDSensorValue(const char* date,const char* value);
		int printToBuff(char* buff);
		virtual size_t printTo(Print&) const;
		
	protected:
		const char* _date;
		const char* _value;
				
};

#endif