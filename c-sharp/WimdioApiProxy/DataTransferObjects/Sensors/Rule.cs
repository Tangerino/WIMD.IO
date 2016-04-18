using System;
using Newtonsoft.Json;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class Rule
    {
        [JsonProperty("enabled")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsEnabled { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isincremental")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsIncremental { get; set; }

        [JsonProperty("checkgap")]
        [JsonConverter(typeof(BoolConverter))]
        public bool CheckGap { get; set; }

        [JsonProperty("loginterval")]
        [JsonConverter(typeof(BoolConverter))]
        public bool LogInterval { get; set; }

        [JsonProperty("indextoabsolute")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IndexToAbsolute { get; set; }

        [JsonProperty("hasminimumvalue")]
        [JsonConverter(typeof(BoolConverter))]
        public bool HasMinimumValue { get; set; }

        [JsonProperty("minimumvalue")]
        public int MinimumValue { get; set; }

        [JsonProperty("hasmaximumvalue")]
        [JsonConverter(typeof(BoolConverter))]
        public bool HasMaximumValue { get; set; }

        [JsonProperty("maximumvalue")]
        public int MaximumValue { get; set; }

        [JsonProperty("lastrun")]
        public DateTime LastRun { get; set; }

        [JsonProperty("hastimezone")]
        [JsonConverter(typeof(BoolConverter))]
        public bool HasTimeZone { get; set; }

        [JsonProperty("tzname")]
        public string TzName { get; set; }
    }

}
