#ifndef WIMDSensorValue_h
/*
  WIMDSensorValue.h - Arduino WIMDClient Library
  Copyright (c) 2016 Sagar Devkota.  All right reserved.
  Email:sagarda7@yahoo.com
*/
#define WIMDSensorValue_h
#include <Stream.h>
#include <Printable.h>

class WIMDSensorValue: public Printable
{
	public:
		WIMDSensorValue(const char* date,const float value);
		int printToBuff(char* buff);
		virtual size_t printTo(Print&) const;
		
	protected:
		const char* _date;
		float _value;
				
};

#endif