using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors
{
    public class NormalizationFactor : NewNormalizationFactor
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
