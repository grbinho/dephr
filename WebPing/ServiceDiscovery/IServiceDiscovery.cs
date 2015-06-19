using System.Threading.Tasks;

namespace WebPing.ServiceDiscovery
{
    public interface IServiceDiscovery
    {
        Task<string> DiscoverServiceAsync(string serviceName);

        string DiscoverService(string serviceName);
    }
}
