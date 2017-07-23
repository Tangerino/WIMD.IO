##############################################
##                                          ##
##       Where Is My Data API Wrapper       ##
##           https://wimd.io/api/           ##
##                                          ##
## ======================================== ##
##  Platform: Python 2.x/3.x                ##
## ======================================== ##
##  Requires:                               ##
##    - Requests 2.9 or higher              ##
##      http://python-requests.org/         ##
##                                          ##
##############################################

'''\
Where Is My Data API Wrapper

Please refer to https://wimd.io/api/ for more info.
'''

'''

-- Date --   Version  Description
2017-03-03    2.1.1   Support for GROUP API
2016-11-11    2.1.0   Support for reports API

'''

__version__ = '2.1.1'

import requests
import json
from requests       import get, put, post, delete
from collections    import namedtuple
from time import time

PERMISSION_READ         = 1
PERMISSION_CREATE       = 2
PERMISSION_UPDATE       = 4
PERMISSION_DELETE       = 8

NF_AGGREGATION_NONE        = 0
NF_AGGREGATION_SUM         = 1
NF_AGGREGATION_AVERAGE     = 2
NF_AGGREGATION_MAXIMUM     = 3
NF_AGGREGATION_MINIMUM     = 4

NF_OPERATION_DIVIDE        = 0
NF_OPERATION_MULTIPLY      = 1

AGGREGATION_NONE        = "none"
AGGREGATION_SUM         = "sum"
AGGREGATION_AVERAGE     = "avg"
AGGREGATION_MAXIMUM     = "max"
AGGREGATION_MINIMUM     = "min"

INTERVAL_ALL            = "all"
INTERVAL_HOUR           = "hour"
INTERVAL_DAY            = "day"
INTERVAL_MONTH          = "month"
INTERVAL_YEAR           = "year"
INTERVAL_LAST           = "last"

STATUS_DOWNLOAD_ERROR   = "DOWNLOAD_ERROR"
STATUS_RECEIVED         = "RECEIVED"
STATUS_EXECUTION_ERROR  = "EXECUTION_ERROR"
STATUS_EXECUTED         = "EXECUTED"

class WIMD():
    "WIMD.IO Client"

    def __init__(self, url = 'http://api.wimd.io/v2'):
        requests.packages.urllib3.disable_warnings()
        self.email       = ''
        self.password    = ''
        self.apikey      = ''
        self.userId      = ''
        self.permissions = ''
        self.url         = url
        self.time        = 0
        return

    def response_time(self):
        return round(self.time, 3)

    def _timer_start(self):
        self.time = time()

    def _timer_stop(self):
        self.time = time()-self.time

    def _apiNode(self, *nodenames):
        url = self.url
        for name in nodenames:
            url += '/' + name
        return url

    def _buildResponse(self, request):
        self._timer_stop()
        data = {}
        success = (request.status_code==200 or request.status_code==201)
        if success:
            try:
                data = request.json()
            except:
                pass
        else:
            try:
                data = dict(statuscode=request.status_code, reason=request.reason,content=request.content)
            except:
                pass
        fieldnames  = ['success']
        if type(data) == dict:
            fieldnames.extend(data.keys())
            return namedtuple('Response', fieldnames)(success, **data)
        else:
            fieldnames.append('data')
            return namedtuple('Response', fieldnames)(success, data)

    def _buildError(self, error):
        self._timer_stop()
        return namedtuple('Error', ['success', 'error'])(False, str(error))

    def _get(self, url, headers=None):
        if headers == None:
            headers = dict(apikey=self.apikey)
        try:
            self._timer_start()
            return self._buildResponse(get(url, headers=headers))
        except Exception as error:
            return self._buildError(error)

    def _post(self, url, data=None, files=None, headers=None):
        if headers == None:
            headers = dict(apikey=self.apikey)
        try:
            self._timer_start()
            return self._buildResponse(post(url, json=data, files=files, headers=headers))
        except Exception as error:
            return self._buildError(error)

    def _put(self, url, data=None, headers=None):
        if headers == None:
            headers = dict(apikey=self.apikey)
        try:
            self._timer_start()
            return self._buildResponse(put(url, json=data, headers=headers))
        except Exception as error:
            return self._buildError(error)

    def _delete(self, url, headers=None):
        if headers == None:
            headers = dict(apikey=self.apikey)
        try:
            self._timer_start()
            return self._buildResponse(delete(url, headers=headers))
        except Exception as error:
            return self._buildError(error)

    def login(self, email, password):
        "Request login into the system"
        response = self._post(self._apiNode('account', 'login'), dict(email=email, password=password), headers={})
        if response.success:
            self.email       = email
            self.password    = password
            self.apikey      = response.apikey
            self.userId      = response.userId
            self.permissions = response.permissions
        return response

    def logout(self):
        """Request system logout"""
        return self._post(self._apiNode('account', 'logout'))

    def changePassword(self, old, new):
        """Change user's password."""
        return self._post(self._apiNode('account', 'password'), dict(old=old, new=new))

    def resetAccount(self):
        """Request account reset."""
        return self._post(self._apiNode('account', 'reset'), dict(email=self.email))

    def createPocket(self, name, **data):
        """Create a application pocket."""
        return self._post(self._apiNode('account', 'pocket', name), data)

    def deletePocket(self, name):
        """Delete pocket."""
        return self._delete(self._apiNode('account', 'pocket', name))

    def readPocket(self, name):
        """Read account pocket content."""
        return self._get(self._apiNode('account', 'pocket', name))

    def searchEntities(self, key):
        """Search the entities for the user."""
        return self._get(self._apiNode('search', key))

    def readUsers(self):
        "List the users for the current user"
        return self._get(self._apiNode('users'))

    def createUser(self, email, password, firstname, lastname, permissions):
        "Creates a new non administrator user"
        return self._post(self._apiNode('user'), dict(email=email, password=password, firstname=firstname, lastname=lastname, permissions=permissions))

    def readUser(self, userid):
        "Read user information"
        return self._get(self._apiNode('user', userid))

    def updateUser(self, userid, firstname, lastname):
        "Update user information"
        return self._put(self._apiNode('user', userid), dict(firstname=firstname, lastname=lastname))

    def deleteUser(self, userid):
        "Delete an user"
        return self._delete(self._apiNode('user', userid))

    def changePermissions(self, userid, permissions):
        "Change user's permissions"
        return self._post(self._apiNode('user', 'permissions', userid), dict(permissions=int(permissions)))

    '''
     Groups
    '''
    def readGroups(self):
        "List the Groups for the current user"
        return self._get(self._apiNode('groups'))

    def createGroup(self, parent, name, description, latitude, longitude, zoom):
        "Creates a new Group in the current user context"
        return self._post(self._apiNode('group'),
                          dict(
                              parent=parent,
                              name=name,
                              description=description,
                              latitude=latitude,
                              longitude=longitude,
                              zoom=zoom
                          )
                          )

    def readGroup(self, groupId):
        "Read Group"
        return self._get(self._apiNode('group', groupId))

    def updateGroup(self, groupId, **info):
        "Update a Group information"
        return self._put(self._apiNode('group', groupId), info)

    def deleteGroup(self, groupId):
        "Delete a Group"
        return self._delete(self._apiNode('group', groupId))

    def linkGroup(self, groupId, userId):
        "Link a Group to an user"
        return self._post(self._apiNode('group', groupId, 'link', userId))

    def unlinkGroup(self, groupId, userId):
        "Unlink a Group from an user"
        return self._delete(self._apiNode('group', groupId, 'link', userId))

    '''
    PLACES
    '''
    def readPlaces(self):
        "List the places for the current user"
        return self._get(self._apiNode('places'))

    def createPlace(self, name, description):
        "Creates a new place in the current user context"
        return self._post(self._apiNode('place'), dict(name=name, description=description))

    def readPlace(self, placeid):
        "Read place"
        return self._get(self._apiNode('place', placeid))

    def updatePlace(self, placeid, **info):
        "Update a place information"
        return self._put(self._apiNode('place', placeid), info)

    def deletePlace(self, placeid):
        "Delete a place"
        return self._delete(self._apiNode('place', placeid))

    def linkPlace(self, placeid, userid):
        "Link a place to an user"
        return self._post(self._apiNode('place', placeid, 'link', userid))

    def unlinkPlace(self, placeid, userid):
        "Unlink a place from an user"
        return self._delete(self._apiNode('place', placeid, 'link', userid))

    def createThing(self, placeid, name, description):
        "Creates a new thing under a place"
        return self._post(self._apiNode('place', placeid, 'thing'), dict(name=name, description=description))

    def readThings(self, placeid):
        "List the things entities for the given place"
        return self._get(self._apiNode('place', placeid, 'things'))

    def createNormalizationFactor(self, placeid, name, description, unit, aggregation, operation):
        "Creates a new normalization factor under the place"
        return self._post(self._apiNode('place', placeid, 'nf'), dict(name=name, description=description, unit=unit, aggregation=aggregation, operation=operation))

    def readNormalizationFactors(self, placeid):
        "List the normalization factors from a place"
        return self._get(self._apiNode('place', placeid, 'nfs'))

    def readNormalizationFactor(self, id):
        "Read normalization factor"
        return self._get(self._apiNode('nf', id))

    def updateNormalizationFoctor(self, nfid, **info):
        "Update a normalization factor"
        return self._put(self._apiNode('nf', nfid), info)

    def deleteNormalizationFactor(self, nfid):
        "Delete a normalization factor"
        return self._delete(self._apiNode('nf', nfid))

    def createNormalizationFactorValue(self, nfid, ts, value):
        "Creates a new normalization factor value"
        return self._post(self._apiNode('nf', nfid, 'value'), dict(ts=ts.strftime('%Y-%m-%d %H:%M:%S'), value=float(value)))

    def updateNormalizationFactorValue(self, nfid, ts, value):
        "Update a normalization factor value"
        return self._put(self._apiNode('nf', nfid, 'value'), dict(ts=ts.strftime('%Y-%m-%d %H:%M:%S'), value=float(value)))

    def deleteNormalizationFactorValue(self, nfid, date):
        "Delete a normalization factor value"
        date = date.strftime('%Y-%m-%d %H:%M:%S')
        return self._delete(self._apiNode('nf', nfid, 'value', date))

    def readNormalizationFactorValues(self, nfid):
        """Read normalization factor values"""
        return self._get(self._apiNode('nf', nfid, 'values'))

    def readThing(self, thingid):
        "List thing attributes"
        return self._get(self._apiNode('thing', thingid))

    def updateThing(self, thingid, **info):
        "Update a thing object"
        return self._put(self._apiNode('thing', thingid), info)

    def deleteThing(self, thingid):
        "Delete a thing"
        return self._delete(self._apiNode('thing', thingid))

    def linkSensor(self, thingid, sensorid):
        "Link a sensor to a thing"
        return self._post(self._apiNode('thing', thingid, 'link', sensorid))

    def unlinkSensor(self, thingid, sensorid):
        "Unlink a sensor from a thing"
        return self._delete(self._apiNode('thing', thingid, 'link', sensorid))

    def listSensors(self, thingid):
        "List all sensors under a thing"
        return self._get(self._apiNode('thing', thingid, 'sensors'))

    def readSensor(self, sensorid):
        "Read sensor"
        return self._get(self._apiNode('sensor', sensorid))

    def readSensorRule(self, sensorid):
        "Read sensor rule"
        return self._get(self._apiNode('sensor', sensorid, 'rule'))

    def updateSensorRule(self, sensorid, **info):
        "Update sensor rule"
        return self._put(self._apiNode('sensor', sensorid, 'rule'), info)

    def readFormulas(self):
        "List formulas that belong to the user"
        return self._get(self._apiNode('formulas'))

    def readFormula(self, formulaid):
        "Read formula information"
        return self._get(self._apiNode('formula', formulaid))

    def readFormulaCode(self, formulaid):
        "Read formula source code"
        return self._get(self._apiNode('formula', formulaid, 'code'))

    def createFormula(self, name, code, library):
        "Creates a new formula in the current user context"
        return self._post(self._apiNode('formula'), dict(name=name, code=code, library=library))

    def updateFormula(self, formulaid, **info):
        "Update a formula information"
        return self._put(self._apiNode('formula', formulaid), info)

    def deleteFormula(self, formulaid):
        "Delete a formula"
        return self._delete(self._apiNode('formula', formulaid))

    def testFormula(self, formulaid, **variables):
        "Test a formula"
        variable_list = list()
        for v in variables:
            variable_list.append(dict(variable=v, value=variables[v]))
        return self._post(self._apiNode('formula', formulaid, 'test'), variable_list)

    def readVirtualSensorVariables(self, sensorid):
        "Read virtual sensor"
        return self._get(self._apiNode('virtual', sensorid))

    def addVirtualSensorVariable(self, sensorid, data):
        "Add a variable (sensor) to the virtual sensor calculation"
        return self._post(self._apiNode('virtual', sensorid, 'link'), data)

    def deleteVirtualSensorVariable(self, vsensorid, sensorid):
        """Delete a variable (sensor) from the virtual sensor calculation"""
        return self._delete(self._apiNode('virtual', vsensorid, 'link', sensorid))

    def readCalendars(self):
        "List calendats that belongs to the user"
        return self._get(self._apiNode('calendars'))

    def createCalendar(self, name):
        "Creates a new calendar in the current user context"
        return self._post(self._apiNode('calendar'), dict(name=name))

    def updateCalendar(self, calendarid, **info):
        "Update a calendar in the current user context"
        return self._put(self._apiNode('calendar', calendarid), info)

    def deleteCalendar(self, calendarid):
        "Delete a calendar in the current user context"
        return self._delete(self._apiNode('calendar', calendarid))

    def readSpecialDays(self, calendarid):
        "List special days that belings to a calendar"
        return self._get(self._apiNode('calendar', calendarid, 'specialdays'))

    def createSpecialDay(self, calendarid, name, day, recurrent):
        "Creates a new special day under a calendar"
        return self._post(self._apiNode('calendar', calendarid, 'specialday'), dict(name=name, day=day.strftime('%Y-%m-%d'), recurrent=recurrent))

    def updateSpecialDay(self, calendarid, specialdayid, **info):
        "Update a special day under a calendar"
        return self._put(self._apiNode('calendar', calendarid, 'specialday', specialdayid), info)

    def deleteSpecialDay(self, calendarid, specialdayid):
        "Delete a special day under a calendar"
        return self._delete(self._apiNode('calendar', calendarid, 'specialday', specialdayid))

    def readSeasons(self, calendarid):
        "List seasons that belongs to calendar"
        return self._get(self._apiNode('calendar', calendarid, 'seasons'))

    def createSeason(self, calendarid, name, startdate):
        "Creates a new season under a calendar"
        startdate = startdate.strftime('%Y-%m-%d')
        return self._post(self._apiNode('calendar', calendarid, 'season'), dict(name=name, start_date=startdate))

    def updateSeason(self, calendarid, seasonid, **info):
        "Update a season under a calendar"
        if 'startdate' in info:
            info['startdate'] = info['startdate'].strftime('%Y-%m-%d')
        return self._put(self._apiNode('calendar', calendarid, 'season', seasonid), info)

    def deleteSeason(self, calendarid, seasonid):
        "Delete a season under a calendar"
        return self._delete(self._apiNode('calendar', calendarid, 'season', seasonid))

    def readPeriods(self, calendarid, seasonid):
        "List periods that belongs to a season"
        return self._get(self._apiNode('calendar', calendarid, 'season', seasonid, 'periods'))

    def createPeriod(self, calendarid, seasonid, name, start_time, end_time, mon=0, tue=0, wed=0, thu=0, fri=0, sat=0, sun=0, spc=0):
        "Creates a new period under a season"
        return self._post(self._apiNode('calendar', calendarid, 'season', seasonid, 'period'), dict(name=name, start_time=start_time, end_time=end_time, mon=mon, tue=tue, wed=wed, thu=thu, fri=fri, sat=sat, sun=sun, spc=spc))

    def updatePeriod(self, calendarid, seasonid, periodid, **info):
        "Update a period under a season"
        return self._put(self._apiNode('calendar', calendarid, 'season', seasonid, 'period', periodid), info)

    def deletePeriod(self, calendarid, seasonid, periodid):
        "Delete a period under a season"
        return self._delete(self._apiNode('calendar', calendarid, 'season', seasonid, 'period', periodid))

    def readRawData(self, sensorid, startdate, enddate):
        "Read raw historical data"
        startdate = startdate.strftime('%Y-%m-%d %H:%M:%S')
        enddate   = enddate.strftime('%Y-%m-%d %H:%M:%S')
        return self._get(self._apiNode('data', sensorid, startdate, enddate, 'raw'))

    def readHistoricalData(self, sensorlistid, startdate, enddate, operation, timeinterval):
        ""
        slist = ""
        if isinstance(sensorlistid, list):
            for id in sensorlistid:
                if not slist :
                    slist = slist + ","
                slist = slist + id
        else:
            slist = sensorlistid
        startdate = startdate.strftime('%Y-%m-%d %H:%M:%S')
        enddate   = enddate.strftime('%Y-%m-%d %H:%M:%S')
        return self._get(self._apiNode('data', slist, startdate, enddate, str(operation), str(timeinterval), 'clean'))

    def readNormalizedData(self, sensorid, startdate, enddate, operation, timeinterval, nfid):
        ""
        startdate = startdate.strftime('%Y-%m-%d %H:%M:%S')
        enddate   = enddate.strftime('%Y-%m-%d %H:%M:%S')
        return self._get(self._apiNode('data', sensorid, startdate, enddate, str(operation), timeinterval, nfid, 'norm'))

    def readCalendarData(self, sensorid, startdate, enddate, operation, timeinterval, calendarid):
        ""
        startdate = startdate.strftime('%Y-%m-%d %H:%M:%S')
        enddate   = enddate.strftime('%Y-%m-%d %H:%M:%S')
        return self._get(self._apiNode('data', sensorid, startdate, enddate, str(operation), timeinterval, calendarid, 'calendar'))

    def readDevices(self):
        "List devices that belongs to the user"
        return self._get(self._apiNode('devices'))

    def readDevice(self, deviceid):
        "Read device information"
        return self._get(self._apiNode('device', deviceid))

    def createDevice(self, name, description):
        "Creates a new device in the current user context"
        return self._post(self._apiNode('device'), dict(name=name, description=description))

    def updateDevice(self, deviceid, **info):
        "Update a device information"
        return self._put(self._apiNode('device', deviceid), info)

    def deleteDevice(self, deviceid):
        "Delete a device"
        return self._delete(self._apiNode('device', deviceid))

    def readSensors(self, deviceid):
        "List the sensors that belongs to a device"
        return self._get(self._apiNode('device', deviceid, 'sensors'))

    def readReportInfo(self, since, limit):
        "List available reports"
        return self._get(self._apiNode('report', 'info', since, str(limit)))

    def readReportBody(self, id):
        "List available reports"
        return self._get(self._apiNode('report', 'body', id))

    def readVirtualSensors(self, deviceid):
        "List all virtual sensors for this device"
        return self._get(self._apiNode('device', deviceid, 'virtualsensors'))

    def createSensor(self, devkey, data):
        "Creates a new sensor"
        return self._post(self._apiNode('sensor'), data, headers=dict(devkey=devkey))

    def updateSensor(self, devkey, remoteid, **info):
        "Update sensor information"
        return self._put(self._apiNode('sensor', remoteid), info)

    def deleteSensor(self, devkey, remoteid):
        "Delete a sensor and its historical data"
        return self._delete(self._apiNode('sensor', remoteid))

    def addSensorsData(self, devkey, data):
        "Add data to sensors"
        return self._post(self._apiNode('sensor', 'data'), data, headers=dict(devkey=devkey))

    def createCommand(self, deviceid, **data):
        "Create command for the shadow device"
        return self._post(self._apiNode('shadow', deviceid), data)

    def deleteCommand(self, deviceid, commandid):
        "Delete pending command for a shadow device"
        return self._delete(self._apiNode('shadow', deviceid, commandid))

    def deviceAcknowledgeCommands(self, devkey, data):
        "Device acknowledge one or more commands"
        return self._post(self._apiNode('commands', 'ack'), data, headers=dict(devkey=devkey))

    def deviceDeleteSettings(self, devkey):
        "Device delete settings"
        return self._delete(self._apiNode('settings'), headers=dict(devkey=devkey))

    def deviceReadCommands(self, devkey, limit):
        "Device read commands from the remote server"
        return self._get(self._apiNode('commands', str(limit)), headers=dict(devkey=devkey))

    def deviceSendSettings(self, devkey, data):
        "Device sends its settings"
        return self._post(self._apiNode('settings'), data, headers=dict(devkey=devkey))

    def readCommands(self, deviceid, startdate, enddate):
        "Read shadow pending commands"
        startdate = startdate.strftime('%Y-%m-%d %H:%M:%S')
        enddate   = enddate.strftime('%Y-%m-%d %H:%M:%S')
        return self._get(self._apiNode('shadow', deviceid, startdate, enddate))

    def readDeviceObject(self, deviceid, objectname, objectinitialid, objectcount):
        "Read shadow device object"
        return self._get(self._apiNode('shadow', deviceid, 'object', objectname, objectinitialid, objectcount))

    def readDeviceObjects(self, deviceid):
        "Read shadow device objects"
        return self._get(self._apiNode('shadow', deviceid, 'objects'))

    def sendFile(self, deviceid, **data):
        "Upload a file to device"
        return self._post(self._apiNode('dropbox', deviceid, 'upload'), data)

    def readFilesInformation(self, deviceid, startdate, enddate):
        "Read files information"
        startdate = startdate.strftime('%Y-%m-%d %H:%M:%S')
        enddate   = enddate.strftime('%Y-%m-%d %H:%M:%S')
        return self._get(self._apiNode('dropbox', deviceid, 'info', startdate, enddate))

    def deleteFile(self, deviceid, fileid):
        "Delete file in queue"
        return self._delete(self._apiNode('dropbox', deviceid, fileid))

    def deviceReadFileInfo(self, devkey):
        "Device requests information about the first available file in queue"
        return self._get(self._apiNode('dropbox', 'device', 'info'), headers=dict(devkey=devkey))

    def deviceAcknowledgeFile(self, devkey, fileid, status, **data):
        "Device acknowledge the reception of the file"
        return self._post(self._apiNode('dropbox', 'device', 'ack', fileid, status), data, headers=dict(devkey=devkey))

    def readETL(self, etlid):
        "Read ETL information"
        return self._get(self._apiNode('etl', etlid))

    def readETLs(self):
        "List the ETLs for the current user"
        return self._get(self._apiNode('etls'))

    def createETLTask(self, name, endpoint, type, placeid, database, username=None, password=None, table=None):
        "Creates a new ETL task"
        return self._post(self._apiNode('etl'), dict(name=name, endpoint=endpoint, type=type, placeid=placeid, database=database, username=username, password=password, table=table))

    def updateETL(self, etlid, **info):
        "Update ETL information"
        return self._put(self._apiNode('etl', etlid), info)

    def deleteETL(self, etlid):
        "Delete an ETL task"
        return self._delete(self._apiNode('etl', etlid))

    def resetETL(self, etlid, date):
        "Reset an ETL to the given date"
        return self._post(self._apiNode('etl', etlid, date.isoformat()))

    def pushEGX300CSV(self, devicekey, filename):
        "EGX300 data push end point"
        return self._post(self._apiNode('gateway', 'egx300', devicekey, 'csv'), files=dict(file=(filename, open(filename, 'rb'))))

    def pushVizeliaCSV(self, devicekey, filename):
        "Vizelia CSV data push end point"
        return self._post(self._apiNode('gateway', 'vizelia', devicekey, 'csv'), files=dict(file=(filename, open(filename, 'rb'))))

    def pushVizeliaXML(self, devicekey, filename):
        """Vizelia XML data push end point"""
        return self._post( files=dict(file=(filename, open(filename, 'rb'))))

def Permission(read=False, create=False, update=False, delete=False):
    "Return a value corresponding to the combined permissions"
    return (PERMISSION_READ   * read  ) + (PERMISSION_CREATE * create) + \
           (PERMISSION_UPDATE * update) + (PERMISSION_DELETE * delete)
           
