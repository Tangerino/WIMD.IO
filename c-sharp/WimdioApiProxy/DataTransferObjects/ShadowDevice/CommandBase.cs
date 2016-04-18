using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public abstract class CommandBase
    {
        [JsonProperty("objectname")]
        public string ObjectName { get; set; }

        [JsonProperty("objectid")]
        public int? ObjectId { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

}
