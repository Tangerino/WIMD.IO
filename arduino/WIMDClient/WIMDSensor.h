#ifndef WIMDSensor_h
/*
  WIMDSensor.h - Arduino WIMDClient Library
  Copyright (c) 2013 Sagar Devkota.  All right reserved.
  Email:sagarda7@yahoo.com
*/
#include <Stream.h>
#include <Printable.h>
#define WIMDSensor_h

class WIMDSensor: public Printable 
{
	public:
		void build(const char* remoteId,const char* name,const char* unitName,const char* unit);
		virtual size_t printTo(Print&) const;
		int printToBuff(char* buff);
		
	protected:
		const char* _remoteId;
		const char* _name;
		const char* _unitName;
		const char* _unit;
		int _length;
				
};

#endif