using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Devices
{
    public class Device : NewDevice
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("tzname")]
        public string TzName { get; set; }

        [JsonProperty("devkey")]
        public string DevKey { get; set; }
    }
}
