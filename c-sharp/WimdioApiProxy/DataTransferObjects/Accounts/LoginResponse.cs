using Newtonsoft.Json;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2.DataTransferObjects.Accounts
{
    public class LoginResponse
    {
        [JsonProperty("apikey")]
        public string ApiKey { get; set; }

        [JsonProperty("permissions")]
        public Permission Permissions { get; set; }
    }
}
