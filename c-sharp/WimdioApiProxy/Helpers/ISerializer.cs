
namespace WimdioApiProxy.v2.Helpers
{
    public interface ISerializer
    {
        T Deserialize<T>(string input) where T : class;
        string Serialize<T>(T input) where T : class;
    }
}
