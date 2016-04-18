using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Things
{
    public class NewThing
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
