/*
  Sensor.cpp - Sensor Class, Arduino WIMDClient Library
  Used to form json body used during creating sensor to the server
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
*/
#include "WIMDSensor.h"


/*
*  Sets parameters used for creating sensor, using only few to save memory
*  @param remoteId, name of sensor, unit name, unit of sensor 
*  @return true if success, false if failed or error
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
void WIMDSensor::build(const char* remoteId,const char* name,const char* unitName,const char* unit){
	_remoteId=remoteId;
	_name=name;
	_unitName=unitName;
	_unit=unit;
}


/*
*  Makes class to be printable via println r print
*  @param Print 
*  @return integer, length of printable
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
size_t WIMDSensor::printTo(Print& aPrint) const
{
  int len = 0;
  len += aPrint.println("{");
  len += aPrint.print("\"remoteid\":\"");
  len += aPrint.print(_remoteId);
  len += aPrint.println("\",");
  len += aPrint.print("\"name\":\"");
  len += aPrint.print(_name);
  len += aPrint.println("\",");
  len += aPrint.print("\"unitname\":\"");
  len += aPrint.print(_unitName);
  len += aPrint.println("\",");
  len += aPrint.println("\"tseoi\":true,");
  //commenting to save RAM
  /*len += aPrint.println("\"rule\":{");
  len += aPrint.println("\"logininterval\":900,");
  len += aPrint.println("\"enabled\":1,");
  len += aPrint.println("\"isincremental\":false,");
  len += aPrint.println("\"hastimezone\":1,");
  len += aPrint.println("\"tzname\":\"Europe/Peris\",");
  len += aPrint.println("\"indextoabsolute\":false,");
  len += aPrint.println("\"checkgap\":false,");
  len += aPrint.println("},");
  */
  len += aPrint.println("\"meta\":\"\",");
  len += aPrint.print("\"unit\":\"");
  len += aPrint.print(_unit);
  len += aPrint.println("\",");
  len += aPrint.println("\"description\":\"\"");
  
  len += aPrint.println("}");
  return len;
}


/*
*  Prints JSON body to buffer
*  @param const char* buf 
*  @return integer, length of json body
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
int WIMDSensor::printToBuff(char* buff)
{
   sprintf(buff+0,"%s","{");
   sprintf(buff+strlen(buff),"%s","\"remoteid\":\"");
   sprintf(buff+strlen(buff),"%s",_remoteId);
   sprintf(buff+strlen(buff),"%s","\",");
   sprintf(buff+strlen(buff),"%s","\"name\":\"");
   sprintf(buff+strlen(buff),"%s",_name);
   sprintf(buff+strlen(buff),"%s","\",");
   sprintf(buff+strlen(buff),"%s","\"unitname\":\"");
   sprintf(buff+strlen(buff),"%s",_unitName);
   sprintf(buff+strlen(buff),"%s","\",");
   sprintf(buff+strlen(buff),"%s","\"tseoi\":true,");
   sprintf(buff+strlen(buff),"%s","\"meta\":\"\",");
   sprintf(buff+strlen(buff),"%s","\"unit\":\"");
   sprintf(buff+strlen(buff),"%s",_unit);
   sprintf(buff+strlen(buff),"%s","\",");
   sprintf(buff+strlen(buff),"%s","\"description\":\"\"");
   sprintf(buff+strlen(buff),"%s","}");
   //memset(buff,'\0',sizeof(buff));
   return strlen(buff);
}







