using Newtonsoft.Json;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class UpdateRule
    {
        public UpdateRule(Rule rule)
        {
            IsEnabled = rule.IsEnabled;
            Name = rule.Name;
            Description = rule.Description;
            IsIncremental = rule.IsIncremental;
            CheckGap = rule.CheckGap;
            LogInterval = rule.LogInterval;
            IndexToAbsolute = rule.IndexToAbsolute;
            HasMinimumValue = rule.HasMinimumValue;
            MinimumValue = rule.MinimumValue;
            HasMaximumValue = rule.HasMaximumValue;
            MaximumValue = rule.MaximumValue;
            HasTimeZone = rule.HasTimeZone;
            TzName = rule.TzName;
        }

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

        [JsonProperty("hastimezone")]
        [JsonConverter(typeof(BoolConverter))]
        public bool HasTimeZone { get; set; }

        [JsonProperty("tzname")]
        public string TzName { get; set; }
    }

}
