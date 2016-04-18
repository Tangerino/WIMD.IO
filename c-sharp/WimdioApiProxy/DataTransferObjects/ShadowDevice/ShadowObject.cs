using Newtonsoft.Json;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class ShadowObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("objectcontent")]
        [JsonConverter(typeof(StringObjectConverter<ShadowObjectContent>))]
        public ShadowObjectContent ObjectContent { get; set; }
    }
}
