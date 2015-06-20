using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebPing.Reporting;

namespace WebPing
{
    public class WebPingConfiguration
    {
        private ICollection<ServiceEndpoint> _endpoints = new Collection<ServiceEndpoint>();
        public ICollection<ServiceEndpoint> Endpoints
        {
            get { return _endpoints; }
            set { _endpoints = value; }
        }

        private Reporters _reporter = Reporters.Trace;
        /// <summary>
        /// If using one of the default reporters, set which one.
        /// </summary>
        public Reporters Reporter
        {
            get { return _reporter; }
            set { _reporter = value; }
        }

        private int _pingInterval = 5000;
        /// <summary>
        /// Interval of hearth beat pings
        /// </summary>
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
