using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors
{
    public class NewNormalizationFactor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("aggregation")]
        public AggregationType Aggregation { get; set; }

        [JsonProperty("operation")]
        public Operation Operation { get; set; }
    }


    public enum AggregationType
    {
        None = 0,
        Sum = 1,
        Average = 2,
        Maximum = 3,
        Minimum = 4
    }

    public enum Operation
    {
        Divide = 0,
        Multiply = 1
    }
}
