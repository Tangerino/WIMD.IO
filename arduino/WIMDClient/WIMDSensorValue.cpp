/*
  Sensor.cpp - Sensor Value Class
  Represents sensor value 
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
*/
#include "WIMDSensorValue.h"


/*
*  Constructor
*  @param datetime, value 
*  @return void
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
WIMDSensorValue::WIMDSensorValue(const char* date,float value):_date(date),_value(value) 
{
  	
}

/*
*  Makes it printable
*  @param Print& 
*  @return length of json
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
size_t WIMDSensorValue::printTo(Print& aPrint) const
{
  int len = 0;
  len += aPrint.print("[");
  len += aPrint.print("\"");
  len += aPrint.print(_date);
  len += aPrint.print("\",");
  len += aPrint.print(_value);
  len += aPrint.print("]");
}


/*
*  Prints value as json to buffer
*  @param char* buff 
*  @return integer, length of value json
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
int WIMDSensorValue::printToBuff(char* buff)
{
   sprintf(buff+0,"%s","[\"");
   sprintf(buff+strlen(buff),"%s",_date);
   sprintf(buff+strlen(buff),"%s","\",");
   sprintf(buff+strlen(buff),"%f",_value);
   sprintf(buff+strlen(buff),"%s","]");
   //memset(buff,'\0',sizeof(buff));
   return strlen(buff);
}







