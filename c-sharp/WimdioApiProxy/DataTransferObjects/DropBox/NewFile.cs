using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WimdioApiProxy.v2.DataTransferObjects.DropBox
{
    public class NewFile
    {
        [JsonProperty(PropertyName= "url")]
        public Uri Url { get; set; }

        [JsonProperty(PropertyName = "action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FileAction Action { get; set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FileType Type { get; set; }
    }

    public enum FileAction
    {
        POST,
        PUT,
        GET,
        DELETE
    }

    public enum FileType
    {
        GENERIC,
        FIRMWARE_UPGRADE,
        CONFIG_FILE,
        SYSTEM_FILE,
        DATABASE
    }
}
