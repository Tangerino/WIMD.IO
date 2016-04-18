using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Etls
{
    public class NewEtl
    {
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

    public enum EtlType
    {
        InfluxDB = 1,
    }
}
