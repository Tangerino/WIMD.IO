using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Users
{
    public class NewUser
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("permissions")]
        public Permission Permissions { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }
    }
}
