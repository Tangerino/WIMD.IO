using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.DataTransferObjects.Etls;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.DataTransferObjects.Formulas;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Places;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Users;
using WimdioApiProxy.v2.DataTransferObjects.DropBox;
using WimdioApiProxy.v2.DataTransferObjects.Sensors;
using WimdioApiProxy.v2.DataTransferObjects.ShadowDevice;
using WimdioApiProxy.v2.DataTransferObjects.Calendars;
using WimdioApiProxy.v2.DataTransferObjects.TimeSeries;

namespace WimdioApiProxy.v2
{
    public partial interface IWimdioApiClient
    {
        Task Login(Credentials credentials);
        Task Logout();
        Task<string> ChangePassword(Credentials credentials);
        Task<string> ResetAccount(Account account);
        Task CreatePocket(string pocketName, object pocket);
        Task DeletePocket(string pocketName);
        Task<dynamic> ReadPocket(string pocketName);

        Task<IEnumerable<User>> ReadUsers();
        Task<User> CreateUser(NewUser user);
        Task<User> ReadUser(Guid userId);
        Task<User> UpdateUser(Guid userId, UpdateUser user);
        Task DeleteUser(Guid userId);
        Task<User> ChangePermissions(Guid userId, Permission permissions);

        Task<IEnumerable<Place>> ReadPlaces();
        Task<Place> CreatePlace(NewPlace place);
        Task<Place> ReadPlace(Guid placeId);
        Task<Place> UpdatePlace(Guid placeId, UpdatePlace place);
        Task DeletePlace(Guid placeId);
        Task LinkPlace(Guid placeId, Guid userId);
        Task UnlinkPlace(Guid placeId, Guid userId);

        Task<IEnumerable<NormalizationFactor>> ReadNormalizationFactors(Guid placeId);
        Task<NormalizationFactor> CreateNormalizationFactor(Guid placeId, NewNormalizationFactor normalizationFactor);
        Task<NormalizationFactor> ReadNormalizationFactor(Guid normalizationFactorId);
        Task<NormalizationFactor> UpdateNormalizationFactor(Guid normalizationFactorId, UpdateNormalizationFactor normalizationFactor);
        Task DeleteNormalizationFactor(Guid normalizationFactorId);

        Task<IEnumerable<NormalizationFactorValue>> ReadNormalizationFactorValues(Guid normalizationFactorId);
        Task<NormalizationFactorValue> CreateNormalizationFactorValue(Guid normalizationFactorId, NormalizationFactorValue normalizationFactorValue);
        Task<NormalizationFactorValue> UpdateNormalizationFactorValue(Guid normalizationFactorId, UpdateNormalizationFactorValue normalizationFactorValue);
        Task DeleteNormalizationFactorValue(Guid normalizationFactorId, DateTime date);

        Task<IEnumerable<Thing>> ReadThings(Guid placeId);
        Task<Thing> CreateThing(Guid placeId, NewThing thing);
        Task<Thing> ReadThing(Guid thingId);
        Task<Thing> UpdateThing(Guid thingId, UpdateThing thing);
        Task DeleteThing(Guid thingId);

        Task<IEnumerable<Device>> ReadDevices();
        Task<Device> CreateDevice(NewDevice device);
        Task<Device> ReadDevice(Guid deviceId);
        Task<Device> UpdateDevice(Guid deviceId, UpdateDevice device);
        Task DeleteDevice(Guid deviceId);

        Task<IEnumerable<Sensor>> ReadSensors(Guid deviceId);
        Task<Sensor> CreateSensor(string devkey, NewSensor device);
        Task<Sensor> ReadSensor(Guid sensorId);
        Task<Sensor> UpdateSensor(string devkey, string remoteId, UpdateSensor device);
        Task DeleteSensor(string devkey, string remoteId);
        Task AddSensorData(string devkey, IEnumerable<SensorSerie> data);
        Task<Rule> ReadSensorRule(Guid sensorId);
        Task<Rule> UpdateSensorRule(Guid sensorId, UpdateRule rule);
        Task<IEnumerable<Sensor>> ListSensors(Guid thingId);
        Task LinkSensor(Guid thingId, Guid sensorId);
        Task UnlinkSensor(Guid thingId, Guid sensorId);
        Task<IEnumerable<SensorVariable>> ReadVirtualSensorVariables(Guid sensorId);
        Task AddVirtualSensorVariables(Guid sensorId, IEnumerable<SensorVariable> variables);
        Task DeleteVirtualSensorVariables(Guid virtualSensorId, string virtualVariableId);
        Task<IEnumerable<Sensor>> ReadVirtualSensors(Guid deviceId);

        Task<IEnumerable<Formula>> ReadFormulas();
        Task<Formula> CreateFormula(NewFormula formula);
        Task<Formula> ReadFormula(Guid formulaId);
        Task<string> ReadFormulaCode(Guid formulaId);
        Task<Formula> UpdateFormula(Guid formulaId, UpdateFormula formula);
        Task DeleteFormula(Guid formulaId);

        Task<IEnumerable<ShadowObjectName>> ReadDeviceObjects(Guid deviceId);
        Task<IEnumerable<ShadowObject>> ReadDeviceObjects(Guid deviceId, string objectName, int objectInitialId = 0, int objectCount = 1);
        Task<CommandDeatils> CreateDeviceCommand(Guid deviceId, NewCommand command);
        Task<IEnumerable<Command>> ReadDeviceCommands(string devkey, int limit);
        Task AcknowledgeDeviceCommands(string devkey, IEnumerable<CommandState> states);
        Task<IEnumerable<CommandDeatils>> ReadDeviceCommands(Guid deviceId, DateTime startDate, DateTime endDate);
        Task DeleteDeviceCommands(Guid deviceId, Guid commandId);
        Task SendDeviceSettings(string devkey, CommandSettings settings);
        Task SendDeviceSettings(string devkey, IEnumerable<ShadowObjectContent> settingRows);

        Task<FileInfo> SendFileToDevice(Guid deviceId, NewFile file);
        Task<IEnumerable<FileInfo>> ReadFilesInformation(Guid deviceId, DateTime startDate, DateTime endDate);
        Task DeleteFile(Guid deviceId, Guid fileId);
        Task<DeviceFileInfo> DeviceReadFileInfo(string devkey);
        Task<bool> DeviceAcknowledgeFile(string devkey, Guid fileId, Status status);

        Task<IEnumerable<Etl>> ReadEtls();
        Task<Etl> CreateEtl(NewEtl etl);
        Task<Etl> ReadEtl(Guid etlId);
        Task<Etl> UpdateEtl(Guid etlId, UpdateEtl etl);
        Task DeleteEtl(Guid etlId);

        Task<Calendar> CreateCalendar(NewCalendar calendar);
        Task<IEnumerable<Calendar>> ReadCalendars();
        Task<Calendar> UpdateCalendar(Guid calendarId, NewCalendar calendar);
        Task DeleteCalendar(Guid calendarId);
        Task<SpecialDay> CreateSpecialDay(Guid calendarId, NewSpecialDay SpecialDay);
        Task UpdateSpecialDay(Guid calendarId, Guid SpecialDayId, NewSpecialDay SpecialDay);
        Task DeleteSpecialDay(Guid calendarId, Guid SpecialDayId);
        Task<Season> CreateSeason(Guid calendarId, NewSeason season);
        Task<IEnumerable<Season>> ReadSeasons(Guid calendarId);
        Task<Season> UpdateSeason(Guid calendarId, Guid seasonId, NewSeason season);
        Task DeleteSeason(Guid calendarId, Guid seasonId);
        Task<Period> CreatePeriod(Guid calendarId, Guid seasonId, NewPeriod period);
        Task<IEnumerable<Period>> ReadPeriods(Guid calendarId, Guid seasonId);
        Task<Period> UpdatePeriod(Guid calendarId, Guid seasonId, Guid periodId, NewPeriod period);
        Task DeletePeriod(Guid calendarId, Guid seasonId, Guid periodId);

        Task<IEnumerable<CalendarData>> ReadCalendarData(Guid sensorId, DateTime startDate, DateTime endDate, DataOperation operation, TimeInterval interval, Guid calendarId);
    }
}
