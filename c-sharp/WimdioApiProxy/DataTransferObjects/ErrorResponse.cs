using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects
{
    public class ErrorResponse : BasicResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
