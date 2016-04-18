using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Places
{
    public class NewPlace
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
