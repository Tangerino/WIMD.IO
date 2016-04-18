##############################################
##                                          ##
##       Where Is My Data API Wrapper       ##
##           https://wimd.io/api/           ##
##                                          ##
## ======================================== ##
##  Version : 2.0.1                         ##
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

__version__ = '2.0.1'

from requests       import get, put, post, delete, exceptions
from collections    import namedtuple
from time import time

PERMISSION_READ         = 1
PERMISSION_CREATE       = 2
PERMISSION_UPDATE       = 4
PERMISSION_DELETE       = 8

AGGREGATION_NONE        = 0
AGGREGATION_SUM         = 1
AGGREGATION_AVERAGE     = 2
AGGREGATION_MAXIMUM     = 3
AGGREGATION_MINIMUM     = 4

OPERATION_DIVIDE        = 0
OPERATION_MULTIPLY      = 1

STATUS_DOWNLOAD_ERROR   = "DOWNLOAD_ERROR"
STATUS_RECEIVED         = "RECEIVED"
STATUS_EXECUTION_ERROR  = "EXECUTION_ERROR"
STATUS_EXECUTED         = "EXECUTED"

__WIMD_IO_BASE_URL = "https://wimd.io/v2"

class WIMD2():
    "WIMD.IO API V2 Client"

    def __init__(self, email, password, debug = False):
        self.email       = email
        self.password    = password
        self.rtt         = 0
        self.debug       = debug
        try:
            response = self.login(email, password)
            if not response.success:
                raise LoginError(response.error)
            self.apikey      = response.apikey
            self.userId      = response.userId
            self.permissions = response.permissions
        except:
            raise

    def login(self, email, password):
        "Request login into the system"
        return self.wimd_post("Login", _apiNode('account', 'login'), json=dict(email=email, password=password))

    def logout(self):
        "Request system logout"
        return self.wimd_post("Logout", _apiNode('account', 'logout'), headers=dict(apikey=self.apikey))

    def changePassword(self):
        "Change user's password"
        return self.wimd_post('ChangePassword', _apiNode('account', 'password'), json=dict(email=self.email, password=self.password))

    def resetAccount(self):
        "Request account reset"
        return self.wimd_post('ResetAccount', _apiNode('account', 'reset'),json=dict(email=self.email))

    def createPocket(self, name, data):
        "Create a application pocket"
        return self.wimd_post('CreatePocket', _apiNode('account', 'pocket', name), headers=dict(apikey=self.apikey), json=data)

    def deletePocket(self, name):
        "Delete pocket"
        return self.wimd_delete('DeletePocket', _apiNode('account', 'pocket', name), headers=dict(apikey=self.apikey))

    def readPocket(self, name):
        "Read account pocket content"
        return self.wimd_get('ReadPocket', _apiNode('account', 'pocket', name), headers=dict(apikey=self.apikey))

    def searchEntities(self, key):
        "Search the entities for the user"
        return self.wimd_get('SearchEntities', _apiNode('search', key), headers=dict(apikey=self.apikey))

    def readUsers(self):
        "List the users for the current user"
        return self.wimd_get('ReadUsers', _apiNode('users'), headers=dict(apikey=self.apikey))

    def createUser(self, email, password, firstname, lastname, permissions):
        "Creates a new non administrator user"
        return self.wimd_post('CreateUser', _apiNode('user'),
                headers=dict(apikey=self.apikey),
                json=dict(email=email, password=password, firstname=firstname,
                          lastname=lastname, permissions=permissions)
                )

    def readUser(self, userid):
        "Read user information"
        return self.wimd_get('ReadUser',
                _apiNode('user', userid),
                headers=dict(apikey=self.apikey))

    def updateUser(self, userid, **info):
        "Update user information"
        return self.wimd_update('UpdateUser', _apiNode('user', userid), headers=dict(apikey=self.apikey), json=info)

    def deleteUser(self, userid):
        "Delete an user"
        return self.wimd_post('DeleteUser',
                _apiNode('user', userid),
                headers=dict(apikey=self.apikey)
                )

    def changePermissions(self, userid, permissions):
        "Change user's permissions"
        return self.wimd_post('ChangePermissions',
                _apiNode('user', 'permissions', userid),
                headers=dict(apikey=self.apikey),
                json=dict(permissions=int(permissions))
                )

    def readPlaces(self):
        "List the places for the current user"
        return self.wimd_get('ReadPlaces',
                _apiNode('places'),
                headers=dict(apikey=self.apikey))

    def createPlace(self, name, description):
        "Creates a new place in the current user context"
        return self.wimd_post('CreatePlace',
                _apiNode('place'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, description=description)
                )

    def readPlace(self, placeuuid):
        "Read place"
        return self.wimd_get('ReadPlace',
                _apiNode('place', placeuuid),
                headers=dict(apikey=self.apikey))

    def updatePlace(self, placeid, **info):
        "Update a place information"
        return self.wimd_update('UpdatePlace',
                _apiNode('place', placeid),
                headers=dict(apikey=self.apikey),
                json=info
                )

    def deletePlace(self, placeid):
        "Delete a place"
        return self.wimd_delete('DeletePlace', _apiNode('place', placeid), headers=dict(apikey=self.apikey))

    def linkPlace(self, placeid, userid):
        "Link a place to an user"
        return self.wimd_post('LinkPlace',
                _apiNode('place', placeid, 'link', userid),
                headers=dict(apikey=self.apikey)
                )

    def unlinkPlace(self, placeid, userid):
        "Unlink a place from an user"
        return self.wimd_delete('UnlinkPlace',
                _apiNode('place', placeid, 'link', userid),
                headers=dict(apikey=self.apikey))

    def createThing(self, placeid, name, description):
        "Creates a new thing under a place"
        return self.wimd_post('CreateThing',
                _apiNode('place', placeid, 'thing'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, description=description))

    def readThings(self, placeid):
        "List the things entities for the given place"
        return self.wimd_get('ReadThings',
                _apiNode('place', placeid, 'things'),
                headers=dict(apikey=self.apikey))

    def createNormalizationFactor(self, placeid, name, description, unit,
                                  aggregation, operation):
        "Creates a new normalization factor under the place"
        return self.wimd_post('CreateNormalizationFactor',
                _apiNode('place', placeid, 'nf'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, description=description, unit=unit,
                          aggregation=aggregation, operation=operation)
                )

    def readNormalizationFactors(self, placeid):
        "List the normalization factors from a place"
        return self.wimd_get('ReadNormalizationFactors',
                _apiNode('place', placeid, 'nfs'),
                headers=dict(apikey=self.apikey))

    def readNormalizationFactor(self, id):
        "Read normalization factor"
        return self.wimd_get('ReadNormalizationFactor',
                _apiNode('nf', id),
                headers=dict(apikey=self.apikey))

    def updateNormalizationFoctor(self, nfid, **info):
        "Update a normalization factor"
        return self.wimd_update('UpdateNormalizationFactor',
                _apiNode('nf', nfid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteNormalizationFactor(self, nfid):
        "Delete a normalization factor"
        return self.wimd_delete('DeleteNormalizationFactor', _apiNode('nf', nfid), headers=dict(apikey=self.apikey))

    def createNormalizationFactorValue(self, nfid, ts, value):
        "Creates a new normalization factor value"
        return self.wimd_post('CreateNormalizationFactorValue',
                _apiNode('nf', nfid, 'value'),
                headers=dict(apikey=self.apikey),
                json=dict(ts=ts, value=value)
                )

    def updateNormalizationFactorValue(self, nfid, **info):
        "Update a normalization factor value"
        return self.wimd_update('UpdateNormalizationFactorValue',
                _apiNode('nf', nfid, 'value'),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteNormalizationFactorValue(self, nfid, date):
        "Delete a normalization factor value"
        return self.wimd_delete('DeleteNormalizationFactorValue',
                _apiNode('nf', nfid, 'value', _ISODate(date)),
                headers=dict(apikey=self.apikey))

    def readNormalizationFactorValues(self, nfid):
        "Read normalization factor values"
        return self.wimd_get('ReadNormalizationFactorValues',
                _apiNode('nf', nfid, 'values'),
                headers=dict(apikey=self.apikey))

    def readThing(self, thingid):
        "List thing attributes"
        return self.wimd_get('ReadThing',
                _apiNode('thing', thingid),
                headers=dict(apikey=self.apikey))

    def updateThing(self, thingid, **info):
        "Update a thing object"
        return self.wimd_update('UpdateThing',
                _apiNode('thing', thingid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteThing(self, thingid):
        "Delete a thing"
        return self.wimd_delete('DeleteThing',
                _apiNode('thing', thingid),
                headers=dict(apikey=self.apikey)
                )

    def linkSensor(self, thingid, sensorid):
        "Link a sensor to a thing"
        return self.wimd_post('LinkSensor',
                _apiNode('thing', thingid, 'link', sensorid),
                headers=dict(apikey=self.apikey))

    def unlinkSensor(self, thingid, sensorid):
        "Unlink a sensor from a thing"
        return self.wimd_delete('UnlinkSensor', _apiNode('thing', thingid, 'link', sensorid), headers=dict(apikey=self.apikey))

    def listSensors(self, thingid):
        "List all sensors under a thing"
        return self.wimd_get('ListSensors',
                _apiNode('thing', thingid, 'sensors'),
                headers=dict(apikey=self.apikey))

    def readSensor(self, sensorid):
        "Read sensor"
        return self.wimd_get('ReadSensor',
                _apiNode('sensor', sensorid),
                headers=dict(apikey=self.apikey))

    def readSensorRule(self, sensorid):
        "Read sensor rule"
        return self.wimd_get('ReadSensorRule',
                _apiNode('sensor', sensorid, 'rule'),
                headers=dict(apikey=self.apikey))

    def updateSensorRule(self, sensorid, **info):
        "Update sensor rule"
        return self.wimd_update('UpdateSensorRule',
                _apiNode('sensor', sensorid, 'rule'),
                headers=dict(apikey=self.apikey),
                json=info)

    def readFormulas(self):
        "List formulas that belong to the user"
        return self.wimd_get('ReadFormulas', _apiNode('formulas'), headers=dict(apikey=self.apikey))

    def readFormula(self, formulaid):
        "Read formula information"
        return self.wimd_get('ReadFormula', _apiNode('formula', formulaid), headers=dict(apikey=self.apikey))

    def readFormulaCode(self, formulaid):
        "Read formula source code"
        return self.wimd_get('ReadFormulaCode', _apiNode('formula', formulaid, 'code'), headers=dict(apikey=self.apikey))

    def createFormula(self, name, code, library):
        "Creates a new formula in the current user context"
        return self.wimd_post('CreateFormula',
                _apiNode('formula'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, code=code, library=library))

    def updateFormula(self, formulaid, **info):
        "Update a formula information"
        return self.wimd_update('UpdateFormula', _apiNode('formula', formulaid), headers=dict(apikey=self.apikey), json=info)

    def deleteFormula(self, formulaid):
        "Delete a formula"
        return self.wimd_delete('DeleteFormula', _apiNode('formula', formulaid), headers=dict(apikey=self.apikey))

    def testFormula(self, formulaid, **variables):
        "Test a formula"
        variable_list = list()
        for v in variables:
            variable_list.append(dict(variable=v, value=variables[v]))
        return self.wimd_post('TestFormula',
                _apiNode('formula', formulaid, 'test'),
                headers=dict(apikey=self.apikey),
                json=variable_list)

    def readVirtualSensorVariables(self, sensorid):
        "Read virtual sensor"
        return self.wimd_get('ReadVirtualSensorVariables', _apiNode('virtual', sensorid), headers=dict(apikey=self.apikey))

    def addVirtualSensorVariable(self, sensorid, data):
        "Add a variable (sensor) to the virtual sensor calculation"
        return self.wimd_post('AddVirtualSensorVariable',
                _apiNode('virtual', sensorid, 'link'),
                headers=dict(apikey=self.apikey),
                json=data)

    def deleteVirtualSensorVariable(self, vsensorid, sensorid):
        "Delete a variable (sensor) from the virtual sensor calculation"
        return self.wimd_delete('DeleteVirtualSensorVariable', _apiNode('virtual', vsensorid, 'link', sensorid), headers=dict(apikey=self.apikey))

    def readCalendars(self):
        "List calendats that belongs to the user"
        return self.wimd_get('ReadCalendars', _apiNode('calendars'), headers=dict(apikey=self.apikey))

    def createCalendar(self, name):
        "Creates a new calendar in the current user context"
        return self.wimd_post('CreateCalendar',
                _apiNode('calendar'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name))

    def updateCalendar(self, calendarid, **info):
        "Update a calendar in the current user context"
        return self.wimd_update('UpdateCalendar',
                _apiNode('calendar', calendarid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteCalendar(self, calendarid):
        "Delete a calendar in the current user context"
        return self.wimd_delete('DeleteCalendar', _apiNode('calendar', calendarid), headers=dict(apikey=self.apikey))

    def readSpecialDays(self, calendarid):
        "List special days that belings to a calendar"
        return self.wimd_get('ReadSpecialDays',
                _apiNode('calendars', calendarid, 'specialdays'),
                headers=dict(apikey=self.apikey)
                )

    def createSpecialDay(self, calendarid, name, day, recurrent):
        "Creates a new special day under a calendar"
        return self.wimd_post('CreateSpecialDay',
                _apiNode('calendar', calendarid, 'specialday'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, day=day, recurrent=recurrent)
                )

    def updateSpecialDay(self, calendarid, specialdayid, **info):
        "Update a special day under a calendar"
        return self.wimd_update('UpdateSpecialDay',
                _apiNode('calendar', calendarid, 'specialday', specialdayid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteSpecialDay(self, calendarid, specialdayid):
        "Delete a special day under a calendar"
        return self.wimd_delete('DeleteSpecialDay',
                _apiNode('calendar', calendarid, 'specialday', specialdayid),
                headers=dict(apikey=self.apikey))

    def readSeasons(self, calendarid):
        "List seasons that belongs to calendar"
        return self.wimd_get('ReadSeasons',
                _apiNode('calendar', calendarid, 'seasons'),
                headers=dict(apikey=self.apikey)
                )

    def createSeason(self, calendarid, name, startdate):
        "Creates a new season under a calendar"
        startdate = _ISODate(startdate)
        return self.wimd_post('CreateSeason',
                _apiNode('calendar', calendarid, 'season'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, start_date=startdate)
                )

    def updateSeason(self, calendarid, seasonid, **info):
        "Update a season under a calendar"
        return self.wimd_update('UpdateSeason',
                _apiNode('calendar', calendarid, 'season', seasonid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteSeason(self, calendarid, seasonid):
        "Delete a season under a calendar"
        return self.wimd_delete('DeleteSeason', _apiNode('calendar', calendarid, 'season', seasonid), headers=dict(apikey=self.apikey))

    def readPeriods(self, calendarid, seasonid):
        "List periods that belongs to a season"
        return self.wimd_get('ReadPeriods',
                _apiNode('calendar', calendarid, 'season', seasonid, 'periods'),
                headers=dict(apikey=self.apikey))

    def createPeriod(self, calendarid, seasonid, name, start_time, end_time,
                     mon=0, tue=0, wed=0, thu=0, fri=0, sat=0, sun=0, spc=0):
        "Creates a new period under a season"
        return self.wimd_post('CreatePeriod',
                _apiNode('calendar', calendarid, 'season', seasonid, 'period'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, start_time=start_time, end_time=end_time,
                          mon=mon, tue=tue, wed=wed, thu=thu,
                          fri=fri, sat=sat, sun=sun, spc=spc))

    def updatePeriod(self, calendarid, seasonid, periodid, **info):
        "Update a period under a season"
        return self.wimd_update('UpdatePeriod',
                _apiNode('calendar', calendarid, 'season', seasonid,
                        'period', periodid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deletePeriod(self, calendarid, seasonid, periodid):
        "Delete a period under a season"
        return self.wimd_delete('DeletePeriod',
                _apiNode('calendar', calendarid, 'season', seasonid,
                        'period', periodid),
                headers=dict(apikey=self.apikey))

    def readRawData(self, sensorid, startdate, enddate):
        "Read raw historical data"
        startdate = _ISODate(startdate)
        enddate   = _ISODate(enddate)
        return self.wimd_get('ReadRawData',
                _apiNode('data', sensorid, startdate, enddate, 'raw'),
                headers=dict(apikey=self.apikey))

    def readCleanData(self, sensorid, startdate, enddate, operation,
                      timeinterval):
        ""
        startdate = _ISODate(startdate)
        enddate   = _ISODate(enddate)
        return self.wimd_get('ReadCleanData',
                _apiNode('data', sensorid, startdate, enddate, operation,
                        timeinterval, 'clean'),
                headers=dict(apikey=self.apikey))

    def readCleanMultipleData(self, sensorid, startdate, enddate, operation, timeinterval):
        ""
        startdate = _ISODate(startdate)
        enddate   = _ISODate(enddate)
        return self.wimd_get('ReadCleanMultipleData',
                _apiNode('mdata', sensorid, startdate, enddate, operation, timeinterval, 'clean'),
                headers=dict(apikey=self.apikey))

    def readNormalizedData(self, sensorid, startdate, enddate, operation,
                           timeinterval, nfid):
        ""
        startdate = _ISODate(startdate)
        enddate   = _ISODate(enddate)
        return self.wimd_get('ReadNormalizedData',
                _apiNode('data', sensorid, startdate, enddate, operation,
                        timeinterval, nfid, 'norm'),
                headers=dict(apikey=self.apikey))

    def readCalendarData(self, sensorid, startdate, enddate, operation,
                         timeinterval, calendarid):
        ""
        startdate = _ISODate(startdate)
        enddate   = _ISODate(enddate)
        return self.wimd_get('ReadCalendarData',
                _apiNode('data', sensorid, startdate, enddate, operation,
                        timeinterval, calendarid, 'calendar'),
                headers=dict(apikey=self.apikey))

    def readDevices(self):
        "List devices that belongs to the user"
        return self.wimd_get('ReadDevices',
                _apiNode('devices'),
                headers=dict(apikey=self.apikey))

    def readDevice(self, deviceid):
        "Read device information"
        return self.wimd_get('ReadDevice',
                _apiNode('device', deviceid),
                headers=dict(apikey=self.apikey))

    def createDevice(self, name, description):
        "Creates a new device in the current user context"
        return self.wimd_post('CreateDevice',
                _apiNode('device'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, description=description))

    def updateDevice(self, deviceid, **info):
        "Update a device information"
        return self.wimd_update('UpdateDevice',
                _apiNode('device', deviceid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteDevice(self, deviceid):
        "Delete a device"
        return self.wimd_delete('DeleteDevice',
                _apiNode('device', deviceid),
                headers=dict(apikey=self.apikey))

    def readSensors(self, deviceid):
        "List the sensors that belongs to a device"
        return self.wimd_get('ReadSensors',
                _apiNode('device', deviceid, 'sensors'),
                headers=dict(apikey=self.apikey))

    def readVirtualSensors(self, deviceid):
        "List all virtual sensors for this device"
        return self.wimd_get('ReadVirtualSensors',
                _apiNode('device', deviceid, 'virtualsensors'),
                headers=dict(apikey=self.apikey))

    def createSensor(self, remoteid, name, description, tseoi):
        "Creates a new sensor"
        return self.wimd_post('CreateSensor',
                _apiNode('sensor'),
                headers=dict(apikey=self.apikey),
                json=dict(remoteid=remoteid, name=name,
                          description=description, tseoi=tseoi))

    def updateSensor(self, remoteid, **info):
        "Update sensor information"
        return self.wimd_update('UpdateSensor',
                _apiNode('sensor', remoteid),
                headers=dict(apikey=self.apikey),
                json=info)

    def deleteSensor(self, remoteid):
        "Delete a sensor and its historical data"
        return self.wimd_delete('DeleteSensor',
                _apiNode('sensor', remoteid),
                headers=dict(apikey=self.apikey))

    def addSensorsData(self, data):
        "Add data to sensors"
        return self.wimd_post('AddSensorsData',
                _apiNode('sensor', 'data'),
                headers=dict(apikey=self.apikey),
                json=data)

    def createCommand(self, deviceid, data):
        "Create command for the shadow device"
        return self.wimd_post('CreateCommand',
                _apiNode('shadow', deviceid),
                headers=dict(apikey=self.apikey),
                json=data)

    def deleteCommand(self, deviceid, commandid):
        "Delete pending command for a shadow device"
        return self.wimd_delete('DeleteCommand',
                _apiNode('shadow', deviceid, commandid),
                headers=dict(apikey=self.apikey))

    def deviceAcknowledgeCommands(self, data):
        "Device acknowledge one or more commands"
        return self.wimd_post('DeviceAcknowledgeCommands',
                _apiNode('commands', 'ack'),
                headers=dict(apikey=self.apikey),
                json=data)

    def deviceDeleteSettings(self):
        "Device delete settings"
        return self.wimd_delete('DeviceDeleteSettings',
                _apiNode('settings'),
                headers=dict(apikey=self.apikey))

    def deviceReadCommands(self, limit):
        "Device read commands from the remote server"
        return self.wimd_get('DeviceReadCommands',
                _apiNode('commands', limit),
                headers=dict(apikey=self.apikey))

    def deviceSendSettings(self, data):
        "Device sends its settings"
        return self.wimd_post('DeviceSendSettings',
                _apiNode('settings'),
                headers=dict(apikey=self.apikey),
                json=data)

    def readCommands(self, deviceid, startdate, enddate):
        "Read shadow pending commands"
        startdate = _ISODate(startdate)
        enddate   = _ISODate(enddate)
        return self.wimd_get('ReadCommands',
                _apiNode('shadow', deviceid, startdate, enddate),
                headers=dict(apikey=self.apikey))

    def readDeviceObject(self, deviceid, objectname,
                         objectinitialid, objectcount):
        "Read shadow device object"
        return self.wimd_get('ReadDeviceObject',
                _apiNode('shadow', deviceid, 'object', objectname,
                        objectinitialid, objectcount),
                headers=dict(apikey=self.apikey))

    def readDeviceObjects(self, deviceid):
        "Read shadow device objects"
        return self.wimd_get('ReadDeviceObjects',
                _apiNode('shadow', deviceid, 'objects'),
                headers=dict(apikey=self.apikey))

    def sendFile(self, deviceid, data):
        "Upload a file to device"
        return self.wimd_post('SendFile',
                _apiNode('dropbox', deviceid, 'upload'),
                headers=dict(apikey=self.apikey),
                json=data)

    def readFilesInformation(self, deviceid, startdate, enddate):
        "Read files information"
        startdate = _ISODate(startdate)
        enddate   = _ISODate(enddate)
        return self.wimd_get('ReadFilesInformation',
                _apiNode('dropbox', deviceid, 'info', startdate, enddate),
                headers=dict(apikey=self.apikey))

    def deleteFile(self, deviceid, fileid):
        "Delete file in queue"
        return self.wimd_delete('DeleteFile', _apiNode('dropbox', deviceid, fileid), headers=dict(apikey=self.apikey))

    def deviceReadFileInfo(self):
        "Device requests information about the first available file in queue"
        return self.wimd_get('DeviceReadFileInfo',
                _apiNode('dropbox', 'device', 'info'),
                headers=dict(apikey=self.apikey))

    def deviceAcknowledgeFile(self, fileid, status, data):
        "Device acknowledge the reception of the file"
        return self.wimd_post('DeviceAcknowledgeFile',
                _apiNode('dropbox', 'device', 'ack', fileid, status),
                headers=dict(apikey=self.apikey),
                json=data)

    def readETL(self, etlid):
        "Read ETL information"
        return self.wimd_get('ReadETL', _apiNode('etl', etlid), headers=dict(apikey=self.apikey))

    def readETLs(self):
        "List the ETLs for the current user"
        return self.wimd_get('ReadETLs', _apiNode('etls'), headers=dict(apikey=self.apikey))

    def createETLTask(self, name, endpoint, type, placeid, database, username=None, password=None, table=None):
        "Creates a new ETL task"
        return self.wimd_post('CreateETLTask',
                _apiNode('etl'),
                headers=dict(apikey=self.apikey),
                json=dict(name=name, endpoint=endpoint, type=type,
                          placeid=placeid, database=database,
                          username=username, password=password, table=table))

    def updateETL(self, etlid, **info):
        "Update ETL information"
        return self.wimd_update('UpdateETL', _apiNode('etl', etlid), headers=dict(apikey=self.apikey), json=info)

    def deleteETL(self, etlid):
        "Delete an ETL task"
        return self.wimd_delete('DeleteETL', _apiNode('etl', etlid), headers=dict(apikey=self.apikey))

    def resetETL(self, etlid, date):
        "Reset an ETL to the given date"
        return self.wimd_post('ResetETL', _apiNode('etl', etlid, date), headers=dict(apikey=self.apikey))

    def pushEGX300CSV(self, devicekey, filename):
        "EGX300 data push end point"
        return self.wimd_post('PushEGX300CSV',
                _apiNode('gateway', 'egx300', devicekey, 'csv'),
                headers=dict(apikey=self.apikey),
                files=dict(file=(filename, open(filename, 'rb'))))

    def pushVizeliaCSV(self, devicekey, filename):
        "Vizelia CSV data push end point"
        return self.wimd_post('PushVizeliaCSV',
                _apiNode('gateway', 'vizelia', devicekey, 'csv'),
                headers=dict(apikey=self.apikey),
                files=dict(file=(filename, open(filename, 'rb'))))

    def pushVizeliaXML(self, devicekey, filename):
        "Vizelia XML data push end point"
        return self.wimd_post('PushVizeliaXML',
                _apiNode('gateway', 'vizelia', devicekey, 'xml'),
                headers=dict(apikey=self.apikey),
                files=dict(file=(filename, open(filename, 'rb'))))

    def wimd_get(self, method, url, params=None, **kwargs):
        try:
            rtt = time()
            r = get(url, params, **kwargs)
            self.rtt = int(round((time() - rtt) * 1000))
            if self.debug:
                print(" GET " + url + " took " + str(self.rtt) + " msec")
            return _buildResponse(method, r)
        except exceptions.RequestException as e:
            raise

    def wimd_post (self, method, url, data=None, json=None, **kwargs):
        try:
            rtt = time()
            r = post(url, data, json, **kwargs)
            self.rtt = int(round((time() - rtt) * 1000))
            if self.debug:
                print("POST " + url + " took " + str(self.rtt) + " msec")
            return _buildResponse(method, r)
        except exceptions.RequestException as e:
            raise

    def wimd_put (self, method, url, data=None, **kwargs):
        try:
            rtt = time()
            r = put(url, data, **kwargs)
            self.rtt = int(round((time() - rtt) * 1000))
            if self.debug:
                print("DELETE " + url + " took " + str(self.rtt) + " msec")
            return _buildResponse(method, r)
        except exceptions.RequestException as e:
            raise

    def wimd_delete (self, method, url, **kwargs):
        try:
            rtt = time()
            r = delete(url, **kwargs)
            self.rtt = int(round((time() - rtt) * 1000))
            if self.debug:
                print("DELETE " + url + " took " + str(self.rtt) + " msec")
            return _buildResponse(method, r)
        except exceptions.RequestException as e:
            raise

class LoginError(Exception):
    pass

def Permission(read=False, create=False, update=False, delete=False):
    "Return a value corresponding to the combined permissions"
    return (PERMISSION_READ   * read  ) + (PERMISSION_CREATE * create) + \
           (PERMISSION_UPDATE * update) + (PERMISSION_DELETE * delete)

def _ISODate(datetime_object):
    "Return ISO-Date str representation of a Python datetime.datetime object"
    return datetime_object.strftime('%Y-%m-%dT%H:%M:%S')

def _apiNode(*nodenames):
    url = __WIMD_IO_BASE_URL
    for name in nodenames:
        url += '/' + name
    return url

def _buildResponse(name, request):
    data        = request.json()
    success     = (request.status_code==200 or request.status_code==201)
    fieldnames  = ['success']
    if type(data) == dict:
        fieldnames.extend(data.keys())
        return namedtuple('%sResponse' %name, fieldnames)(success, **data)
    else:
        fieldnames.append('data')
        return namedtuple('%sResponse' %name, fieldnames)(success, data)
