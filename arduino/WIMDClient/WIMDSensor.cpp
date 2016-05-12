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

/*
  Sensor.cpp - Sensor Class, Arduino WIMDClient Library
  Used to form json body used during creating sensor to the server
*/
#include "WIMDSensor.h"


/*
*  Sets parameters used for creating sensor, using only few to save memory
*  @param remoteId, name of sensor, unit name, unit of sensor 
*  @return true if success, false if failed or error
*  @since 1.0
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
   return strlen(buff);
}







