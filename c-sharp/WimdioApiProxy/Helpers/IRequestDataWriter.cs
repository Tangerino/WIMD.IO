using System.Net;
using System.Threading.Tasks;

namespace WimdioApiProxy.v2.Helpers
{
    public interface IRequestDataWriter
    {
        Task WriteAsync(HttpWebRequest request, string serializedObject);
    }
}
