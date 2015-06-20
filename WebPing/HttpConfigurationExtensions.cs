using System.Web.Hosting;
using System.Web.Http;
using Dephr.Reporting;
using Dephr.ServiceDiscovery;
using System.Collections.Generic;

namespace Dephr
{
    public static class HttpConfigurationExtensions
    {
        public static void EnableDephr(this HttpConfiguration config, 
            DephrConfiguration DephrConfig, IList<IHearthBeatReporter> reporters)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                var pinger = new Pinger(DephrConfig, reporters);
                pinger.Monitor();
            });
        }

        public static void EnableDephr(this HttpConfiguration config, 
            DephrConfiguration DephrConfig, 
            IList<IHearthBeatReporter> reporters, 
            IServiceDiscovery serviceDiscovery)
        {
            //Check if it's already running, multiple calls.. multiple checks
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                var pinger = new Pinger(DephrConfig, reporters, serviceDiscovery);
                pinger.Monitor();
            });
        }

    }
}
