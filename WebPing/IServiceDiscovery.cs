using System.Threading.Tasks;

namespace WebPing
{
    public interface IServiceDiscovery
    {
        Task<string> DiscoverServiceAsync(string serviceName);

        string DiscoverService(string serviceName);
    }
}
