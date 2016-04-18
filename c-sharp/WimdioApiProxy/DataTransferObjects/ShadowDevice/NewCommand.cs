using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class NewCommand : CommandBase
    {
        [JsonProperty("action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandAction Action { get; set; }

        [JsonProperty("objectcontent")]
        [JsonConverter(typeof(StringObjectConverter<ShadowObjectContent>))]
        public ShadowObjectContent ObjectContent { get; set; }
    }

    public enum CommandAction
    {
        CREATE,
        READ,
        UPDATE,
        DELETE
    }
}
