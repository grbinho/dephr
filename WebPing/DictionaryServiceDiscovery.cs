using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPing
{
    public class DictionaryServiceDiscovery : IServiceDiscovery
    {
        private readonly IDictionary<string, string> _serviceMap;

        public DictionaryServiceDiscovery(IDictionary<string, string> serviceMap)
        {
            _serviceMap = serviceMap;
        }

        public string DiscoverService(string serviceName)
        {
            var serviceNameKey = _serviceMap.Keys.FirstOrDefault(s => s.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase));

            if (serviceNameKey == null)
            {
                return null;
            }

            var serviceEndpoint = _serviceMap[serviceNameKey];

            return serviceEndpoint;
        }


        public Task<string> DiscoverServiceAsync(string serviceName)
        {
            return Task.Factory.StartNew(() => DiscoverService(serviceName));
        }
    }
}
