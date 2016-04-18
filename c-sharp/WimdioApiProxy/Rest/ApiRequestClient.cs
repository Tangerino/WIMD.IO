
namespace WimdioApiProxy.v2.Rest
{
    internal class ApiRequestClient : AuthenticationClient
    {
        public ApiRequestClient(string baseUrl, string apiKey, int? requestTimeoutInSeconds = null)  : base(baseUrl, requestTimeoutInSeconds)
        {
            if (apiKey != null)
            {
                CustomHeaders.Add("apikey", apiKey);
            }
        }

        public void AddCustomHeader(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            CustomHeaders.Add(key, value);
        }
    }
}
