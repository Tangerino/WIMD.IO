using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Accounts
{
    public class Credentials : Account
    {
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
