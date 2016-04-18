using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Formulas
{
    public class UpdateFormula : FormulaCode
    {
        public UpdateFormula(Formula formula)
        {
            Name = formula.Name;
            Library = formula.Library;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("library")]
        public int Library { get; set; }
    }
}
