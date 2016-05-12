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
  Sensor.cpp - Sensor Value Class
  Represents sensor value 
*/
#include "WIMDSensorValue.h"


/*
*  Constructor
*  @param datetime, value 
*  @return void
*  @since 1.0
*/
WIMDSensorValue::WIMDSensorValue(const char* date,const char* value):_date(date),_value(value) 
{
  	
}

/*
*  Makes it printable
*  @param Print& 
*  @return length of json
*  @since 1.0
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
*/
int WIMDSensorValue::printToBuff(char* buff)
{
   sprintf(buff+0,"%s","[\"");
   sprintf(buff+strlen(buff),"%s",_date);
   sprintf(buff+strlen(buff),"%s","\",");
   sprintf(buff+strlen(buff),"%s",_value);
   sprintf(buff+strlen(buff),"%s","]");
   return strlen(buff);
}







