using System.Web.Hosting;
using System.Web.Http;
using WebPing.Reporting;
using WebPing.ServiceDiscovery;

namespace WebPing
{
    public static class HttpConfigurationExtensions
    {
        public static void EnableWebPing(this HttpConfiguration config, 
            WebPingConfiguration webPingConfig, IHearthBeatReporter reporter)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                var pinger = new Pinger(webPingConfig, reporter);
                pinger.Monitor();
            });
        }

        public static void EnableWebPing(this HttpConfiguration config, WebPingConfiguration webPingConfig, IHearthBeatReporter reporter, IServiceDiscovery serviceDiscovery)
        {
            //Check if it's already running, multiple calls.. multiple checks
            HostingEnvironment.QueueBackgroundWorkItem(ct =>
            {
                var pinger = new Pinger(webPingConfig, reporter, serviceDiscovery);
                pinger.Monitor();
            });
        }

    }
}
