#ifndef WIMDRequest_h
/*
  WIMDRequest.h - Arduino WIMDClient Library
  Copyright (c) 2016 wimd.io.  All right reserved.
  Author:sagarda7@yahoo.com
*/
#define WIMDRequest_h
#include "WIMDFeed.h"


class WIMDRequest
{
	public:
		WIMDRequest(WIMDFeed* feeds, int feedsCount);
		int size() { return _feedsCount; };
		WIMDFeed& operator[] (unsigned i) { return _feeds[i]; };
		

		
	//protected:
		int _feedsCount;
		WIMDFeed* _feeds;
		
				
};

#endif