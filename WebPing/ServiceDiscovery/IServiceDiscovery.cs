using System.Threading.Tasks;

namespace Dephr.ServiceDiscovery
{
    public interface IServiceDiscovery
    {
        Task<string> DiscoverServiceAsync(string serviceName);

        string DiscoverService(string serviceName);
    }
}
