using Newtonsoft.Json;
using System.Collections.Generic;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class CommandSettings
    {
        [JsonProperty("settings")]
        public string Settings { get; set; }

        [JsonProperty("objects")]
        public IEnumerable<SettingsObject> Objects { get; set; }
    }

    public class SettingsObject
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("rows")]
        public IEnumerable<ShadowObjectContent> Rows { get; set; }
    }
}
