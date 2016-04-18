using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.Rest
{
    internal class AuthenticationClient : BaseClient
    {
        public AuthenticationClient(string baseUrl, int? requestTimeoutInSeconds = null)
            : base(baseUrl: baseUrl,
                   dataSerializer: new JsonSerializer(),
                   responseDataReader: new ResponseDataReader(),
                   requestDataWriter: new AsciiRequestWriter(),
                   requestTimeoutInSeconds: requestTimeoutInSeconds)
        {
            ContentType = "application/json";
        }
    }
}
