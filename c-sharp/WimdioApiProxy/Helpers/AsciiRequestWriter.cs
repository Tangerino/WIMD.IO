using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WimdioApiProxy.v2.Helpers
{
    public class AsciiRequestWriter : IRequestDataWriter
    {
        public async Task WriteAsync(HttpWebRequest request, string serializedObject)
        {
            var bytes = Encoding.ASCII.GetBytes(serializedObject);
            request.ContentLength = bytes.Length;

            using (var s = request.GetRequestStream())
            {
                await s.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
