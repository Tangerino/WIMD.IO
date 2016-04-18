using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Formulas
{
    public class Formula : NewFormula
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
