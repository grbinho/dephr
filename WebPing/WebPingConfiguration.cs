using System.Collections.Generic;
using WebPing.Reporters;

namespace WebPing
{
    public class WebPingConfiguration
    {
        public ICollection<string> ServiceNames { get; set; }

        public IDictionary<string, string> ServiceMap { get; set; }

        public DefaultReporters Reporter { get; set; }

        public int PingInterval { get; set; }

        /// <summary>
        /// After this time has passed, Pinger will do service descovery again. Time in seconds.
        /// </summary>
        public int ServiceDiscoveryTTL { get; set; }
    }
}
