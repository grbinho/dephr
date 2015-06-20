using Service1.Areas.Monitoring;
using System.Web.Http;
using Dephr;
using Dephr.Reporting;
using System.Collections.Generic;

namespace Service1
{
    public static class WebPingConfig
    {
        public static void Register(this HttpConfiguration config)
        {
            var webPingConfiguration = new DephrConfiguration();
            //TODO: Make this part better
            webPingConfiguration.Endpoints.Add(new ServiceEndpoint {
                Name = "Service1",
                Url = "http://localhost:8090/"
            });
            webPingConfiguration.PingInterval = 5000;
            webPingConfiguration.ServiceDiscoveryTTL = 30;

            config.EnableDephr(webPingConfiguration, new List<IHearthBeatReporter> {
                new SignalRReporter(),
                new TraceReporter()
            });
        }
    }
}
