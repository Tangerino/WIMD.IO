using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Etls
{
    public class UpdateEtl
    {
        public UpdateEtl(Etl etl)
        {
            Name = etl.Name;
            Endpoint = etl.Endpoint;
            Username = etl.Username;
            Password = etl.Password;
            Type = etl.Type;
            PlaceId = etl.PlaceId;
            DatabaseName = etl.DatabaseName;
            TableName = etl.TableName;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("endpoint")]
        public Uri Endpoint { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("type")]
        public EtlType Type { get; set; }

        [JsonProperty("placeid")]
        public Guid? PlaceId { get; set; }

        [JsonProperty("database")]
        public string DatabaseName { get; set; }

        [JsonProperty("table")]
        public string TableName { get; set; }
    }
}
