using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPing.ServiceDiscovery
{
    public class DefaultServiceDiscovery : IServiceDiscovery
    {
        private readonly ICollection<ServiceEndpoint> _endpoints;

        public DefaultServiceDiscovery(ICollection<ServiceEndpoint> endpoints)
        {
            if(endpoints == null)
            {
                throw new ArgumentException("endpoints");
            }

            _endpoints = endpoints;
        }

        public string DiscoverService(string serviceName)
        {
            var serviceEndpoint = _endpoints
                .Where(s => s.Name.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                .Select(s => s.Url)
                .FirstOrDefault();

            return serviceEndpoint;
        }


        public Task<string> DiscoverServiceAsync(string serviceName)
        {
            return Task.Factory.StartNew(() => DiscoverService(serviceName));
        }
    }
}
