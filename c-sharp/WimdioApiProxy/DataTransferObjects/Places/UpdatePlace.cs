using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Places
{
    public class UpdatePlace
    {
        public UpdatePlace (Place place)
        {
            Name = place.Name;
            Description = place.Description;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
