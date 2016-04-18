using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class ShadowObjectName
    {
        [JsonProperty("objectname")]
        public string ObjectName { get; set; }
    }
}
