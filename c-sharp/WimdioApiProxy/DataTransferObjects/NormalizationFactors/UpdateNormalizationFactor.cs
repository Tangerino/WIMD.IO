using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors
{
    public class UpdateNormalizationFactor
    {
        public UpdateNormalizationFactor(NormalizationFactor nf)
        {
            Name = nf.Name;
            Description = nf.Description;
            Unit = nf.Unit;
            Aggregation = nf.Aggregation;
            Operation = nf.Operation;
        }

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
}
