using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using WebPing.Reporters;
using WebPing.Utils;

namespace WebPing
{
    public class Pinger
    {
        private readonly IHearthBeatReporter _reporter;

        //TODO: Decide how long to cache service descovery results. (Configuration)
        private readonly IServiceDiscovery _serviceDiscovery;

        private readonly WebPingConfiguration _config;

        private IDictionary<string, string> _serviceEndpointsCache;

        private DateTime _nextServiceDisovery;

        private ICircuitBreaker _serviceDiscoveryCircuit;
        private ICircuitBreaker _pingCircuit;


        public Pinger(WebPingConfiguration config) 
            : this(config, DefaultReporterFactory.CreateReporter(config.Reporter), new DictionaryServiceDiscovery(config.ServiceMap)) {}

        public Pinger(WebPingConfiguration config, IHearthBeatReporter reporter)
            : this(config, reporter, new DictionaryServiceDiscovery(config.ServiceMap)) {}

        public Pinger(WebPingConfiguration config, IHearthBeatReporter reporter, IServiceDiscovery serviceDiscovery)
        {
            _serviceEndpointsCache = new Dictionary<string, string>();
            _config = config;
            _reporter = reporter;
            _serviceDiscovery = serviceDiscovery;
            _nextServiceDisovery = DateTime.UtcNow;
            _serviceDiscoveryCircuit = new PollyCircuitBreaker(new OptimisticCircuitBreakerConfiguration());
            _pingCircuit = new PollyCircuitBreaker(new OptimisticCircuitBreakerConfiguration());
        }


        /// <summary>
        /// Gets endpoint for service name. If discovery timeout has passed, uses service discovery and caches result. If not returns result from cache.
        /// </summary>
        /// <param name="serviceName">Name of the service</param>
        private bool tryGetEndpoint(string serviceName, out string endpoint)
        {
            bool ttlExpired = false;

            //Check service disovery timeout (TTL)
            if (DateTime.UtcNow > _nextServiceDisovery)
            {
                ttlExpired = true;
                _nextServiceDisovery = DateTime.UtcNow.AddSeconds(_config.ServiceDiscoveryTTL);
            }

            if (ttlExpired || !_serviceEndpointsCache.Keys.Any(k => k.Equals(serviceName)))
            {
                endpoint = null;            

                try
                {
                    endpoint = _serviceDiscoveryCircuit.Execute(() => _serviceDiscovery.DiscoverService(serviceName));
                }
                catch (CircuitBrokenException ex)
                {
                    //This will be invoked every time we try to call execute and circuit is open.
                    //Circuit colses depending on the setting.
                    Trace.WriteLine(string.Format("Circuit broke when discovering a service: {0}.", serviceName));
                }

                if(endpoint == null)
                {
                    return false;
                }
                else
                {
                    //Refresh cache
                    _serviceEndpointsCache[serviceName] = endpoint;
                }
            }
            else
            {
                endpoint = _serviceEndpointsCache
                    .Where(s => s.Key.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                    .Select(s => s.Value).First();
            }

            return true;
        }

        private HearthBeat pingEndpoint(HttpClient client, string serviceName)
        {
            string endpoint;

            var endpointExists = tryGetEndpoint(serviceName, out endpoint));
            var beat = new HearthBeat(serviceName, endpoint, 0, false);

            if(!endpointExists)
            {
                return beat;
            }

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                //TODO: Test this idea.. vs. Do sync web request.. Could be cheaper.
                var resultTask = _pingCircuit.ExecuteAsync(() => client.GetAsync(endpoint));
                resultTask.Wait();

                stopwatch.Stop();

                var result = resultTask.Result;
                beat = new HearthBeat(serviceName, endpoint, stopwatch.ElapsedMilliseconds, result.IsSuccessStatusCode);
            }
            catch(CircuitBrokenException ex)
            {
                Trace.WriteLine(string.Format("Circuit broke when during a hearth bead event for a service: {0}.", serviceName));
            }

            return beat;
        }

        /// <summary>
        /// Blocks and pings all configured services. Reports state by using configured reporter
        /// </summary>
        public void Monitor()
        {
            var client = new HttpClient();
            //Run hearth beating in sequence (for the first version)

            while(true)
            {
                foreach (var service in _config.ServiceNames)
                {
                    //Ping the endpoint (GET request)
                    var beat = pingEndpoint(client, service);
                    _reporter.Report(beat);
                }

                //Configurable ping interval
                Thread.Sleep(_config.PingInterval);
            }
        }
    }
}
