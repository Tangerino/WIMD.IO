using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Devices
{
    public class UpdateDevice
    {
        public UpdateDevice(Device device)
        {
            Name = device.Name;
            Description = device.Description;
            Mac = device.Mac;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("mac")]
        public string Mac { get; set; }
    }
}
