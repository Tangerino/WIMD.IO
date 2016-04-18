using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WimdioApiProxy.v2.Tests
{
    public abstract class BaseTests
    {
        private static IWimdioApiClient _client;
        protected static IWimdioApiClient Client
        {
            get
            {
                if (_client == null)
                    _client = Task.Run(() => GetAuthorizedClient()).Result;

                return _client;
            }
        }

        protected static readonly Credentials Credentials = new Credentials
        {
            Email = "email here",
            Password = "password here"
        };

        internal static async Task<IWimdioApiClient> GetAuthorizedClient()
        {
            var client = new WimdioApiClient();
            await client.Login(Credentials);
            return client;
        }

        internal static async Task<User> CreateUser(IWimdioApiClient client)
        {
            var user = new NewUser
            {
                Password = "secure",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = $"dummy+{Guid.NewGuid().ToString().Split('-').First()}@email.com",
                Permissions = Permission.Read | Permission.Update
            };

            return await client.CreateUser(user);
        }

        internal static async Task<Place> CreatePlace(IWimdioApiClient client)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var place = new NewPlace
            {
                Name = $"Name {random}",
                Description = $"Description {random}"
            };

            return await client.CreatePlace(place);
        }

        internal static async Task<NormalizationFactor> CreateNormalizationFactor(IWimdioApiClient client, Place place)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var normalizationFactor = new NewNormalizationFactor
            {
                Name = $"Name {random}",
                Description = $"Description {random}",
                Aggregation = AggregationType.Average,
                Operation = Operation.Divide,
                Unit = $"Unit {random}"
            };

            var created = await client.CreateNormalizationFactor(place.Id, normalizationFactor);

            return created;
        }

        internal static async Task<NormalizationFactorValue> CreateNormalizationFactorValue(IWimdioApiClient client, NormalizationFactor nf)
        {
            var dateTime = DateTime.Now;
            var rnd = new Random();

            var normalizationFactorValue = new NormalizationFactorValue
            {
                Timestamp = dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond)),
                Value = rnd.Next(100000).ToString(),
            };

            return await client.CreateNormalizationFactorValue(nf.Id, normalizationFactorValue);
        }

        internal static async Task<Thing> CreateThing(IWimdioApiClient client, Place place)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var thing = new NewThing
            {
                Name = "Name " + random,
                Description = "Description " + random
            };

            return await client.CreateThing(place.Id, thing);
        }

        internal static async Task<Device> CreateDevice(IWimdioApiClient client)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var device = new NewDevice
            {
                Name = $"Name {random}",
                Description = $"Description {random}",
                Mac = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20)
            };

            return await client.CreateDevice(device);
        }

        internal static async Task<Sensor> CreateSensor(IWimdioApiClient client, Device device, bool isVirtual = false)
        {
            var guid = Guid.NewGuid();
            var random = guid.ToString().Split('-').First();

            var newSensor = new NewSensor
            {
                RemoteId = guid.ToString(),
                Name = $"Name {random}",
                Description = $"Description {random}",
                Unit = "ppm",
                Tseoi = 0,
                IsVirtual = isVirtual
            };

            return await client.CreateSensor(device.DevKey, newSensor);
        }

        internal static async Task<Formula> CreateFormula(IWimdioApiClient client)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var formula = new NewFormula
            {
                Name = $"Name {random}",
                Code = $"ww = w * w\r\nvv = v * v\r\nr = math.sqrt(ww + vv)\r\nvm = w / r",
                Library = 0
            };

            var created = await client.CreateFormula(formula);
            created.Code = formula.Code;

            return created;
        }

        internal static async Task<FileInfo> CreateFile(IWimdioApiClient client, Device device)
        {
            var file = new NewFile
            {
                Url = new Uri("http://veryshorthistory.com/wp-content/uploads/2015/04/knights-templar.jpg"),
                Action = FileAction.POST,
                Type = FileType.FIRMWARE_UPGRADE
            };

            return await client.SendFileToDevice(device.Id, file);
        }

        internal static async Task<Etl> CreateEtl(IWimdioApiClient client, Place place)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var etl = new NewEtl
            {
                Name = $"Name {random}",
                Endpoint = new Uri("http://www.google.com"),
                Username = $"Username{random}",
                Password = $"{random}",
                Type = EtlType.InfluxDB,
                PlaceId = place.Id,
                DatabaseName = $"Database{random}",
                TableName = $"Table{random}",
            };

            return await client.CreateEtl(etl);
        }

        internal static SensorData CreateSensorData(IEnumerable<string> remoteIds)
        {
            var data = new SensorData
            {
                Series = remoteIds.Select(x => new SensorSerie { RemoteId = x }).ToList()
            };

            var now = DateTime.Now;
            now = now.AddMilliseconds(-now.Millisecond);
            var rnd = new Random();

            data.Series.ForEach(x => 
            {
                x.AddValue(now.AddSeconds(0), Math.Round(rnd.NextDouble() * 100, 2));
                x.AddValue(now.AddSeconds(1), Math.Round(rnd.NextDouble() * 100, 2));
            });

            return data;
        }

        internal static CommandSettings CarlosTestSettings()
        {
            return new CommandSettings
            {
                Settings = "edge",
                Objects = new List<SettingsObject>
                {
                    new SettingsObject
                    {
                        Object = "cloudservice",
                        Rows = new List<ShadowObjectContent>
                        {
                            new ShadowObjectContent
                            {
                                Id = 20,
                                Name = "wimd",
                                Type = 18,
                                Enabled = true,
                                Host = 1,
                                PublishInterval = 1440,
                                TagPosition = 31310287,
                                LastRun = DateTime.Parse("2016-03-19 08:46:05"),
                                NextRun = DateTime.MaxValue,
                                Status = 200,
                                Pause = false,
                                ZipIt = false,
                                ApiKey = null,
                                CleanSession = false,
                                Timeout = 30,
                                ActivationCode = Guid.Parse("d109a897-d26f-11e5-8d5d-04017fd5d401"),
                                FeedId = null,
                                MailTo = null,
                                MailCc = null,
                                MailBcc = null,
                                EventPosition = 0,
                                AlarmPosition = 0,
                                ConfigPosition = 2
                            }
                        }
                    }
                }
            };
        }
    }
}
