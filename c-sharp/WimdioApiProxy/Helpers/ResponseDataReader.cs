using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace WimdioApiProxy.v2.Helpers
{
    public class ResponseDataReader : IResponseDataReader
    {
        public async Task<string> ReadAsync(HttpWebResponse response)
        {
            var responseString = string.Empty;

            using (var r = new StreamReader(response.GetResponseStream()))
            {
                responseString = await r.ReadToEndAsync();
            }

            return responseString;
        }
    }
}
