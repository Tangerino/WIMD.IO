#ifndef WIMDSensor_h
/*
  WIMDSensor.h - Arduino WIMDClient Library
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
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
		const char* getId() {return _remoteId;}
		
	protected:
		const char* _remoteId;
		const char* _name;
		const char* _unitName;
		const char* _unit;
		int _length;
				
};

#endif