using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class NewSensor
    {
        [JsonProperty("remoteid")]
        public string RemoteId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("tseoi")]
        public int Tseoi { get; set; }

        [JsonProperty("is_virtual")]
        public bool IsVirtual { get; set; }
    }
}
