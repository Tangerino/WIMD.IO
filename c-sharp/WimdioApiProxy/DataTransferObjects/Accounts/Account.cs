using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Accounts
{
    public class Account
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
