using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Things
{
    public class UpdateThing
    {
        public UpdateThing(Thing thing)
        {
            Name = thing.Name;
            Description = thing.Description;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
