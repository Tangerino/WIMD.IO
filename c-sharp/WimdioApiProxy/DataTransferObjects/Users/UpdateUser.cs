using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Users
{
    public class UpdateUser
    {
        public UpdateUser(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }
    }
}
