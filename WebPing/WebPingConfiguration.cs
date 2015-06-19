using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebPing.Reporters;

namespace WebPing
{
    public class WebPingConfiguration
    {
        private ICollection<string> _serviceNames = new Collection<string>();
        public ICollection<string> ServiceNames
        {
            get { return _serviceNames; }
            set { _serviceNames = value; }
        }

        private IDictionary<string, string> _serviceMap = new Dictionary<string, string>();
        public IDictionary<string, string> ServiceMap
        {
            get { return _serviceMap; }
            set { _serviceMap = value; }
        }

        private DefaultReporters _reporter = DefaultReporters.Trace;

        public DefaultReporters Reporter
        {
            get { return _reporter; }
            set { _reporter = value; }
        }

        private int _pingInterval = 5000;
        public int PingInterval
        {
            get { return _pingInterval; }
            set { _pingInterval = value; }
        }

        private int _serviceDiscoveryTTL = 300; //Five minutes
        /// <summary>
        /// After this time has passed, Pinger will do service descovery again. Time in seconds.
        /// </summary>
        public int ServiceDiscoveryTTL
        {
            get { return _serviceDiscoveryTTL; }
            set { _serviceDiscoveryTTL = value; }
        }
    }
}
