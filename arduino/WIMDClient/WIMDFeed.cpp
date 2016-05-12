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
  WIMDFeed.cpp - A class used to form json body which is ready to be passed to WIMD 
  server while adding sensor data
*/
#include "WIMDFeed.h"

/*
*  Constructor
*  @param remoteId, array of SensorValues, number of values 
*  @return void
*  @since 1.0
*/
WIMDFeed::WIMDFeed(const char* remoteId,WIMDSensorValue* sensorValues, int sensorsCount):_remoteId(remoteId),_sensorValues(sensorValues),_sensorsCount(sensorsCount) 
{
  	
}


/*
*  Virtual method, Makes class to be printable via print() or println() methods
*  @param Print 
*  @return integer, length of printable message
*  @since 1.0
*/
size_t WIMDFeed::printTo(Print& aPrint) const
{
  int len = 0;
  len += aPrint.println("{");
  len += aPrint.print("\"id\":\"");
  len += aPrint.print(_remoteId);
  len += aPrint.println("\",");
  len += aPrint.println("\"values\" : [");
  for (int j =0; j < _sensorsCount; j++)
  {
    len += aPrint.print(_sensorValues[j]);
    if (j == _sensorsCount-1)
    {
      // Last time through
      len += aPrint.println();
    }
    else
    {
      len += aPrint.println(",");
    }
  }
  len += aPrint.println("]");
  len += aPrint.println("}");

  return len;
}











