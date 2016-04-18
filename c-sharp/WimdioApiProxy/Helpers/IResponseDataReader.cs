using System.Net;
using System.Threading.Tasks;

namespace WimdioApiProxy.v2.Helpers
{
    public interface IResponseDataReader
    {
        Task<string> ReadAsync(HttpWebResponse response);
    }
}
