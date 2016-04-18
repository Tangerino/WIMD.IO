using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects
{
    public class BasicResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
