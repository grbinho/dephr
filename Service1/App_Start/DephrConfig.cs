using Service1.Areas.Monitoring;
using System.Web.Http;
using Dephr;
using Dephr.Reporting;
using System.Collections.Generic;

namespace Service1
{
    public static class DephrConfig
    {
        public static void Register(this HttpConfiguration config)
        {
            var dephrConfiguration = new DephrConfiguration();

            dephrConfiguration.Endpoints.Add(new ServiceEndpoint {
                Name = "Service1",
                Url = "http://localhost:8090/"
            });

            dephrConfiguration.Endpoints.Add(new ServiceEndpoint {
                Name = "IBMBluemix",
                Url = "http://testservice1.eu-gb.mybluemix.net/monitor/ping"
            });

            dephrConfiguration.PingInterval = 5000;
            dephrConfiguration.ServiceDiscoveryTTL = 30;

            config.EnableDephr(dephrConfiguration, new List<IHearthBeatReporter> {
                new SignalRReporter(),
                new TraceReporter()
            });
        }
    }
}
