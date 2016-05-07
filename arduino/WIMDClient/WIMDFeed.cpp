/*
  WIMDFeed.cpp - A class used to form json body which is ready to be passed to WIMD 
  server while adding sensor data
  Copyright (c) 2016 Sagar Devkota.  All right reserved.
  Email:sagarda7@yahoo.com
*/
#include "WIMDFeed.h"

/*
*  Constructor
*  @param remoteId, array of SensorValues, number of values 
*  @return void
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
*/
WIMDFeed::WIMDFeed(const char* remoteId,WIMDSensorValue* sensorValues, int sensorsCount):_remoteId(remoteId),_sensorValues(sensorValues),_sensorsCount(sensorsCount) 
{
  	
}


/*
*  Virtual method, Makes class to be printable via print() or println() methods
*  @param Print 
*  @return integer, length of printable message
*  @since 1.0
*  Author Sagar Devkota<sagarda7@yahoo.com> 
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











