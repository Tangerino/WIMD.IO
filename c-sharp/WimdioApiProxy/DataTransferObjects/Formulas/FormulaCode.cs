using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Formulas
{
    public class FormulaCode
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
