using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Formulas
{
    public class NewFormula : FormulaCode
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("library")]
        public int Library { get; set; }
    }
}
