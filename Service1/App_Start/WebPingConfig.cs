using System.Web.Http;
using WebPing;

namespace Service1
{
    public static class WebPingConfig
    {
        public static void Register(this HttpConfiguration config)
        {
            var webPingConfiguration = new WebPingConfiguration();
            //TODO: Make this part better
            webPingConfiguration.Endpoints.Add(new ServiceEndpoint {
                Name = "Service1",
                Url = "http://localhost:8090/"
            });
            webPingConfiguration.PingInterval = 5000;
            webPingConfiguration.ServiceDiscoveryTTL = 30;

            config.EnableWebPing(webPingConfiguration);
        }
    }
}
