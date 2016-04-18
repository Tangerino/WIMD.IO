using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WimdioApiProxy.v2.Rest;
using WimdioApiProxy.v2.DataTransferObjects;
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
    public class WimdioApiClient : IWimdioApiClient
    {
        private const string _baseUrl = "https://wimd.io/v2";
        private string _apiKey;

        private readonly ILog _log = LogManager.GetLogger(typeof(WimdioApiClient));

        public async Task Login(Credentials credentials)
        {
            try
            {
                var client = new AuthenticationClient(_baseUrl);

                _apiKey = (await client.Post<LoginResponse>("account/login", credentials))?.ApiKey;
            }
            catch (Exception ex)
            {
                _log.Error($"Login(credentials.Email={credentials.Email}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task Logout()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Post<BasicResponse>("account/logout", new EmptyObject());

                _apiKey = null;
            }
            catch (Exception ex)
            {
                _log.Error($"Logout() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<string> ChangePassword(Credentials credentials)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/password", credentials);

                return response.Status;
            }
            catch (Exception ex)
            {
                _log.Error($"ChangePassword(credentials.Email={credentials.Email}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<string> ResetAccount(Account account)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/reset", account);

                return response.Status;
            }
            catch (Exception ex)
            {
                _log.Error($"ResetAccount(account.Email={account.Email}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task CreatePocket(string pocketName, object pocket)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>($"account/pocket/{pocketName}", pocket);
            }
            catch (Exception ex)
            {
                _log.Error($"CreatePocket(pocketName={pocketName}, pocket='{JsonConvert.SerializeObject(pocket)}') failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeletePocket(string pocketName)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Delete<BasicResponse>($"account/pocket/{pocketName}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeletePocket(pocketName={pocketName}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<dynamic> ReadPocket(string pocketName)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<object>($"account/pocket/{pocketName}");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPocket(pocketName={pocketName}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<User>> ReadUsers()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<User[]>("users");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadUsers() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> CreateUser(NewUser user)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<User[]>("user", user))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateUser(user={JsonConvert.SerializeObject(user)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> ReadUser(Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<User[]>($"user/{userId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadUser(userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> UpdateUser(Guid userId, UpdateUser user)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<User[]>($"user/{userId}", user))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateUser(userId={userId}, user={JsonConvert.SerializeObject(user)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteUser(Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"user/{userId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteUser(userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<User> ChangePermissions(Guid userId, Permission permissions)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<User[]>($"user/permissions/{userId}", new { permissions }))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ChangePermissions(userId={userId}, permissions) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Place>> ReadPlaces()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<Place[]>("places");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPlaces() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Place> CreatePlace(NewPlace place)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Place[]>("place", place))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreatePlace(place={JsonConvert.SerializeObject(place)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Place> ReadPlace(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Place[]>($"place/{placeId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPlace(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Place> UpdatePlace(Guid placeId, UpdatePlace place)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Place[]>($"place/{placeId}", place))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdatePlace(placeId={placeId}, user={JsonConvert.SerializeObject(place)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeletePlace(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"place/{placeId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeletePlace(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task LinkPlace(Guid placeId, Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Post<BasicResponse>($"place/{placeId}/link/{userId}", new EmptyObject());
            }
            catch (Exception ex)
            {
                _log.Error($"LinkPlace(placeId={placeId}, userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task UnlinkPlace(Guid placeId, Guid userId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"place/{placeId}/link/{userId}");
            }
            catch (Exception ex)
            {
                _log.Error($"UnlinkPlace(placeId={placeId}, userId={userId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<NormalizationFactor>> ReadNormalizationFactors(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<NormalizationFactor[]>($"place/{placeId}/nfs");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadNormalizationFactors(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactor> CreateNormalizationFactor(Guid placeId, NewNormalizationFactor normalizationFactor)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<NormalizationFactor[]>($"place/{placeId}/nf", normalizationFactor))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateNormalizationFactor(placeId={placeId}, normalizationFactor={JsonConvert.SerializeObject(normalizationFactor)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactor> ReadNormalizationFactor(Guid normalizationFactorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<NormalizationFactor[]>($"nf/{normalizationFactorId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadNormalizationFactor(normalizationFactorId={normalizationFactorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactor> UpdateNormalizationFactor(Guid normalizationFactorId, UpdateNormalizationFactor normalizationFactor)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<NormalizationFactor[]>($"nf/{normalizationFactorId}", normalizationFactor))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateNormalizationFactor(normalizationFactorId={normalizationFactorId}, normalizationFactor={JsonConvert.SerializeObject(normalizationFactor)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteNormalizationFactor(Guid normalizationFactorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"nf/{normalizationFactorId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteNormalizationFactor(normalizationFactorId={normalizationFactorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<NormalizationFactorValue>> ReadNormalizationFactorValues(Guid normalizationFactorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<NormalizationFactorValue[]>($"nf/{normalizationFactorId}/values");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadNormalizationFactorValues(normalizationFactorId={normalizationFactorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactorValue> CreateNormalizationFactorValue(Guid normalizationFactorId, NormalizationFactorValue normalizationFactorValue)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Post<BasicResponse>($"nf/{normalizationFactorId}/value", normalizationFactorValue);

                return normalizationFactorValue;
            }
            catch (Exception ex)
            {
                _log.Error($"CreateNormalizationFactorValue(normalizationFactorId={normalizationFactorId}, normalizationFactorValue={JsonConvert.SerializeObject(normalizationFactorValue)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<NormalizationFactorValue> UpdateNormalizationFactorValue(Guid normalizationFactorId, UpdateNormalizationFactorValue normalizationFactorValue)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Put<BasicResponse>($"nf/{normalizationFactorId}/value", normalizationFactorValue);

                return (await ReadNormalizationFactorValues(normalizationFactorId))?.FirstOrDefault(x => x.Timestamp == normalizationFactorValue.Timestamp);
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateNormalizationFactorValue(normalizationFactorId={normalizationFactorId}, normalizationFactorValue={JsonConvert.SerializeObject(normalizationFactorValue)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteNormalizationFactorValue(Guid normalizationFactorId, DateTime date)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"nf/{normalizationFactorId}/value/{date.ToString("o")}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteNormalizationFactorValue(normalizationFactorId={normalizationFactorId}, date={date.ToString("o")}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Thing>> ReadThings(Guid placeId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<Thing[]>($"place/{placeId}/things");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadThings(placeId={placeId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Thing> CreateThing(Guid placeId, NewThing thing)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Thing[]>($"place/{placeId}/thing", thing))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateThing(thing={JsonConvert.SerializeObject(thing)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Thing> ReadThing(Guid thingId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Thing[]>($"thing/{thingId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadThing(thingId={thingId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Thing> UpdateThing(Guid thingId, UpdateThing thing)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Thing[]>($"thing/{thingId}", thing))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateThing(thingId={thingId}, thing={JsonConvert.SerializeObject(thing)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteThing(Guid thingId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"thing/{thingId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteThing(thingId={thingId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Device>> ReadDevices()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<Device[]>($"devices");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadDevices() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Device> CreateDevice(NewDevice device)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Device[]>($"device", device))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateDevice(device={JsonConvert.SerializeObject(device)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Device> ReadDevice(Guid deviceId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Device[]>($"device/{deviceId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadDevice(deviceId={deviceId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Device> UpdateDevice(Guid deviceId, UpdateDevice device)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Device[]>($"device/{deviceId}", device))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateDevice(deviceId={deviceId}, device={JsonConvert.SerializeObject(device)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteDevice(Guid deviceId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"device/{deviceId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteDevice(deviceId={deviceId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Sensor>> ReadSensors(Guid deviceId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<Sensor[]>($"device/{deviceId}/sensors");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadSensors(deviceId={deviceId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Sensor> CreateSensor(string devkey, NewSensor sensor)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);

                return (await client.Post<Sensor[]>($"sensor", sensor))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateSensor(devkey={devkey}, sensor={JsonConvert.SerializeObject(sensor)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Sensor> ReadSensor(Guid sensorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Sensor[]>($"sensor/{sensorId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadSensor(sensorId={sensorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Sensor> UpdateSensor(string devkey, string remoteId, UpdateSensor sensor)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);

                return (await client.Put<Sensor[]>($"sensor/{remoteId}", sensor))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateSensor(devkey={devkey}, remoteId={remoteId}, sensor={JsonConvert.SerializeObject(sensor)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteSensor(string devkey, string remoteId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);

                await client.Delete<BasicResponse>($"sensor/{remoteId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteSensor(devkey={devkey}, remoteId={remoteId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task AddSensorData(string devkey, IEnumerable<SensorSerie> data)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);

                await client.Post<BasicResponse>($"sensor/data", data);
            }
            catch (Exception ex)
            {
                _log.Error($"SensorAddData(devkey={devkey}, data={JsonConvert.SerializeObject(data)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Rule> ReadSensorRule(Guid sensorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Rule[]>($"sensor/{sensorId}/rule"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadSensorRule(sensorId={sensorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Rule> UpdateSensorRule(Guid sensorId, UpdateRule rule)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Rule[]>($"sensor/{sensorId}/rule", rule))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateSensorRule(sensorId={sensorId}, rule={JsonConvert.SerializeObject(rule)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<Sensor>> ListSensors(Guid thingId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<Sensor[]>($"thing/{thingId}/sensors");
            }
            catch (Exception ex)
            {
                _log.Error($"ListSensors(thingId={thingId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task LinkSensor(Guid thingId, Guid sensorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Post<BasicResponse>($"thing/{thingId}/link/{sensorId}", new EmptyObject());
            }
            catch (Exception ex)
            {
                _log.Error($"LinkSensor(thingId={thingId}, sensorId={sensorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task UnlinkSensor(Guid thingId, Guid sensorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"thing/{thingId}/link/{sensorId}");
            }
            catch (Exception ex)
            {
                _log.Error($"UnlinkSensor(thingId={thingId}, sensorId={sensorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<SensorVariable>> ReadVirtualSensorVariables(Guid sensorId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<SensorVariable[]>($"virtual/{sensorId}");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadVirtualSensorVariables(sensorId={sensorId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task AddVirtualSensorVariables(Guid sensorId, IEnumerable<SensorVariable> variables)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Post<BasicResponse>($"virtual/{sensorId}/link", variables);
            }
            catch (Exception ex)
            {
                _log.Error($"AddVirtualSensorVariables(sensorId={sensorId}, variables={JsonConvert.SerializeObject(variables)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteVirtualSensorVariables(Guid virtualSensorId, string virtualVariableId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"virtual/{virtualSensorId}/link/{virtualVariableId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteVirtualSensorVariables(virtualSensorId={virtualSensorId}, virtualVariableId={virtualVariableId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<Sensor>> ReadVirtualSensors(Guid deviceId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<Sensor[]>($"device/{deviceId}/virtualsensors");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadVirtualSensors(deviceId={deviceId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Formula>> ReadFormulas()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<Formula[]>("formulas");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadFormulas() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Formula> CreateFormula(NewFormula formula)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                var created = (await client.Post<Formula[]>("formula", formula))?.FirstOrDefault();
                formula.Code = await ReadFormulaCode(created.Id);
                return created;
            }
            catch (Exception ex)
            {
                _log.Error($"CreateFormula(formula={JsonConvert.SerializeObject(formula)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Formula> ReadFormula(Guid formulaId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                var formula = (await client.Get<Formula[]>($"formula/{formulaId}"))?.FirstOrDefault();
                formula.Code = await ReadFormulaCode(formula.Id);
                return formula;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadFormula(formulaId={formulaId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<string> ReadFormulaCode(Guid formulaId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<FormulaCode[]>($"formula/{formulaId}/code"))?.FirstOrDefault()?.Code;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadFormulaCode(formulaId={formulaId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Formula> UpdateFormula(Guid formulaId, UpdateFormula formula)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                var updated = (await client.Put<Formula[]>($"formula/{formulaId}", formula))?.FirstOrDefault();
                updated.Code = await ReadFormulaCode(updated.Id);
                return updated;
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateFormula(formulaId={formulaId}, user={JsonConvert.SerializeObject(formula)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteFormula(Guid formulaId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"formula/{formulaId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteFormula(formulaId={formulaId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<ShadowObjectName>> ReadDeviceObjects(Guid deviceId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                return await client.Get<ShadowObjectName[]>($"shadow/{deviceId}/objects");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadDeviceObjects(deviceId={deviceId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<ShadowObject>> ReadDeviceObjects(Guid deviceId, string objectName, int objectInitialId = 0, int objectCount = 1)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                return await client.Get<ShadowObject[]>($"shadow/{deviceId}/object/{objectName}/{objectInitialId}/{objectCount}");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadDeviceObjects(deviceId={deviceId}, objectName={objectName}, objectInitialId={objectInitialId}, objectCount={objectCount}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<CommandDeatils> CreateDeviceCommand(Guid deviceId, NewCommand command)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                return (await client.Post<CommandDeatils[]>($"shadow/{deviceId}", command))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateDeviceCommand(deviceId={deviceId}, command={JsonConvert.SerializeObject(command)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<Command>> ReadDeviceCommands(string devkey, int limit)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);
                return await client.Get<Command[]>($"commands/{limit}");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadDeviceCommands(devkey={devkey}, limit={limit}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task AcknowledgeDeviceCommands(string devkey, IEnumerable<CommandState> states)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);
                await client.Post<BasicResponse>($"commands/ack", states);
            }
            catch (Exception ex)
            {
                _log.Error($"AcknowledgeDeviceCommands(devkey={devkey}, states={JsonConvert.SerializeObject(states)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<CommandDeatils>> ReadDeviceCommands(Guid deviceId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                return await client.Get<CommandDeatils[]>($"shadow/{deviceId}/{startDate.ToString("o")}/{endDate.ToString("o")}");
            }
            catch (Exception ex)
            {
                _log.Error($"CreateDeviceCommands(deviceId={deviceId}, startDate={startDate.ToString("o")}, endDate={endDate.ToString("o")}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteDeviceCommands(Guid deviceId, Guid commandId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                await client.Delete<BasicResponse>($"shadow/{deviceId}/{commandId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteDeviceCommands(deviceId={deviceId}, commandId={commandId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task SendDeviceSettings(string devkey, CommandSettings settings)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);

                await client.Post<BasicResponse>($"settings", settings);
            }
            catch (Exception ex)
            {
                _log.Error($"SendDeviceSettings(devkey={devkey}, file={JsonConvert.SerializeObject(settings)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task SendDeviceSettings(string devkey, IEnumerable<ShadowObjectContent> settingRows)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);
                client.AddCustomHeader(nameof(devkey), devkey);

                await client.Post<BasicResponse>($"settings", settingRows);
            }
            catch (Exception ex)
            {
                _log.Error($"SendDeviceSettings(devkey={devkey}, file={JsonConvert.SerializeObject(settingRows)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<FileInfo> SendFileToDevice(Guid deviceId, NewFile file)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                if (file.Type == FileType.FIRMWARE_UPGRADE)
                {
                    await client.Post<BasicResponse>($"dropbox/{deviceId}/upload", file);
                    return null;
                }
                else
                {
                    return (await client.Post<FileInfo[]>($"dropbox/{deviceId}/upload", file))?.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _log.Error($"SendFileToDevice(deviceId={deviceId}, file={JsonConvert.SerializeObject(file)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<FileInfo>> ReadFilesInformation(Guid deviceId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<FileInfo[]>($"dropbox/{deviceId}/info/{startDate.ToString("o")}/{endDate.ToString("o")}");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadFilesInformation(deviceId={deviceId}, startDate={startDate.ToString("o")}, endDate={endDate.ToString("o")}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteFile(Guid deviceId, Guid fileId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"dropbox/{deviceId}/{fileId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteFile(deviceId={deviceId}, fileId={fileId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<DeviceFileInfo> DeviceReadFileInfo(string devkey)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                client.AddCustomHeader(nameof(devkey), devkey);

                return (await client.Get<DeviceFileInfo[]>($"dropbox/device/info"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"DeviceReadFileInfo(devkey={devkey}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<bool> DeviceAcknowledgeFile(string devkey, Guid fileId, Status status)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                client.AddCustomHeader(nameof(devkey), devkey);

                var response = await client.Post<BasicResponse>($"dropbox/device/ack/{fileId}/{status}", new EmptyObject());

                return response.Code == 200;
            }
            catch (Exception ex)
            {
                _log.Error($"DeviceAcknowledgeFile(devkey={devkey}, fileId={fileId}, status={status}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<Etl>> ReadEtls()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<Etl[]>("etls");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadEtls() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Etl> CreateEtl(NewEtl etl)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Etl[]>("etl", etl))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateEtl(etl={JsonConvert.SerializeObject(etl)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Etl> ReadEtl(Guid etlId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Get<Etl[]>($"etl/{etlId}"))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"ReadEtl(etlId={etlId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Etl> UpdateEtl(Guid etlId, UpdateEtl etl)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Etl[]>($"etl/{etlId}", etl))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateEtl(etlId={etlId}, user={JsonConvert.SerializeObject(etl)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteEtl(Guid etlId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"etl/{etlId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteEtl(etlId={etlId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<Calendar> CreateCalendar(NewCalendar calendar)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Calendar[]>("calendar", calendar))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateCalendar(calendar={JsonConvert.SerializeObject(calendar)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<Calendar>> ReadCalendars()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<IEnumerable<Calendar>>("calendars");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadCalendars() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Calendar> UpdateCalendar(Guid calendarId, NewCalendar calendar)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Calendar[]>($"calendar/{calendarId}", calendar))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateCalendar(calendarId={calendarId}, calendar={JsonConvert.SerializeObject(calendar)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteCalendar(Guid calendarId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"calendar/{calendarId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteCalendar(calendarId={calendarId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<SpecialDay> CreateSpecialDay(Guid calendarId, NewSpecialDay specialDay)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<SpecialDay[]>($"calendar/{calendarId}/specialday", specialDay))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateSpecialDay(calendarId={calendarId}, specialDay={JsonConvert.SerializeObject(specialDay)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<SpecialDay>> ReadSpecialDays()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<IEnumerable<SpecialDay>>("specialdays");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadSpecialDays() failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task UpdateSpecialDay(Guid calendarId, Guid specialDayId, NewSpecialDay specialDay)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Put<SpecialDay[]>($"calendar/{calendarId}/specialday/{specialDayId}", specialDay);
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateSpecialDay(calendarId={calendarId}, specialDayId={specialDayId}, specialDay={JsonConvert.SerializeObject(specialDay)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteSpecialDay(Guid calendarId, Guid specialDayId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"calendar/{calendarId}/specialday/{specialDayId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteSpecialDay(calendarId={calendarId}, specialDayId={specialDayId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<Season> CreateSeason(Guid calendarId, NewSeason season)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Season[]>($"calendar/{calendarId}/season", season))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreateSeason(calendarId={calendarId}, season={JsonConvert.SerializeObject(season)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<Season>> ReadSeasons(Guid calendarId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<IEnumerable<Season>>($"calendar/{calendarId}/seasons");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadSeasons(calendarId={calendarId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Season> UpdateSeason(Guid calendarId, Guid seasonId, NewSeason season)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Season[]>($"calendar/{calendarId}/season/{seasonId}", season))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdateSeason(calendarId={calendarId}, seasonId={seasonId}, season={JsonConvert.SerializeObject(season)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeleteSeason(Guid calendarId, Guid seasonId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"calendar/{calendarId}/season/{seasonId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeleteSeason(calendarId={calendarId}, seasonId={seasonId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<Period> CreatePeriod(Guid calendarId, Guid seasonId, NewPeriod period)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Post<Period[]>($"calendar/{calendarId}/season/{seasonId}/period", period))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"CreatePeriod(calendarId={calendarId}, seasonId={seasonId}, period={JsonConvert.SerializeObject(period)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<Period>> ReadPeriods(Guid calendarId, Guid seasonId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<IEnumerable<Period>>($"calendar/{calendarId}/season/{seasonId}/periods");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPeriods(calendarId={calendarId}, seasonId={seasonId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task<Period> UpdatePeriod(Guid calendarId, Guid seasonId, Guid periodId, NewPeriod period)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return (await client.Put<Period[]>($"calendar/{calendarId}/season/{seasonId}/period/{periodId}", period))?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error($"UpdatePeriod(calendarId={calendarId}, seasonId={seasonId}, periodId={periodId}, period={JsonConvert.SerializeObject(period)}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
        public async Task DeletePeriod(Guid calendarId, Guid seasonId, Guid periodId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                await client.Delete<BasicResponse>($"calendar/{calendarId}/season/{seasonId}/period/{periodId}");
            }
            catch (Exception ex)
            {
                _log.Error($"DeletePeriod(calendarId={calendarId}, seasonId={seasonId}, periodId={periodId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<CalendarData>> ReadCalendarData(Guid sensorId, DateTime startDate, DateTime endDate, DataOperation operation, TimeInterval interval, Guid calendarId)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                return await client.Get<IEnumerable<CalendarData>>($"data/{sensorId}/{startDate.ToString("o")}/{endDate.ToString("o")}/{operation.ToString().ToLower()}/{interval.ToString().ToLower()}/{calendarId}/calendar");
            }
            catch (Exception ex)
            {
                _log.Error($"ReadCalendarData(sensorId={sensorId}, startDate={startDate}, endDate={endDate}, operation={operation}, interval={interval}, calendarId={calendarId}) failed", ex);

                throw new WimdioApiClientException(ex.Message, ex);
            }
        }
    }
}
