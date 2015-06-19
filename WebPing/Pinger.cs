using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private IDictionary<string, string> _serviceEndpointsUrlCache;
        private DateTime _nextServiceDisovery;
        private ICircuitBreaker _serviceDiscoveryCircuit;
        private ICircuitBreaker _pingCircuit;

        public Pinger(WebPingConfiguration config) 
            : this(config, DefaultReporterFactory.CreateReporter(config.Reporter), new DefaultServiceDiscovery(config.Endpoints)) {}

        public Pinger(WebPingConfiguration config, IHearthBeatReporter reporter)
            : this(config, reporter, new DefaultServiceDiscovery(config.Endpoints)) {}

        public Pinger(WebPingConfiguration config, IHearthBeatReporter reporter, IServiceDiscovery serviceDiscovery)
        {
            _serviceEndpointsUrlCache = new Dictionary<string, string>();
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
        private bool tryGetEndpoint(string serviceName, out string endpointUrl)
        {
            bool ttlExpired = false;

            //Check service disovery timeout (TTL)
            if (DateTime.UtcNow > _nextServiceDisovery)
            {
                ttlExpired = true;
                _nextServiceDisovery = DateTime.UtcNow.AddSeconds(_config.ServiceDiscoveryTTL);
            }

            if (ttlExpired || !_serviceEndpointsUrlCache.Keys.Any(k => k.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase)))
            {
                endpointUrl = null;            

                try
                {
                    endpointUrl = _serviceDiscoveryCircuit.Execute(() => _serviceDiscovery.DiscoverService(serviceName));
                }
                catch (CircuitBrokenException ex)
                {
                    //This will be invoked every time we try to call execute and circuit is open.
                    //Circuit colses depending on the setting.
                    Trace.WriteLine(string.Format("Circuit broke when discovering a service: {0}. Exception: {1}", serviceName, ex));
                }

                if(endpointUrl == null)
                {
                    return false;
                }
                else
                {
                    //Refresh cache
                    _serviceEndpointsUrlCache[serviceName] = endpointUrl;
                }
            }
            else
            {
                endpointUrl = _serviceEndpointsUrlCache
                    .Where(s => s.Key.Equals(serviceName, StringComparison.InvariantCultureIgnoreCase))
                    .Select(s => s.Value).First();
            }

            return true;
        }

        private HearthBeat pingEndpoint(ServiceEndpoint service)
        {
            string endpointUrl;

            var endpointExists = tryGetEndpoint(service.Name, out endpointUrl);
            service.Url = endpointUrl;
            var beat = new HearthBeat(service, 0, false);

            if(!endpointExists)
            {
                return beat;
            }

            try
            {
                var stopwatch = new Stopwatch();

                stopwatch.Start();
                //TODO: Test this idea.. vs. Do sync web request.. Could be cheaper.
                var result = _pingCircuit.Execute(() => doRequest(service.Url, service.Method, service.SuccessStatus));

                stopwatch.Stop();

                beat = new HearthBeat(service, stopwatch.ElapsedMilliseconds, result);
            }
            catch (CircuitBrokenException ex)
            {
                Trace.WriteLine(string.Format("Circuit broke when during a hearth bead event for a service: {0}. Exception: {1}", service.Name, ex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Exception occured when calling endpoint: {0}. Exception: {1}", endpointUrl, ex));
            }

            return beat;
        }

        /// <summary>
        /// Blocks and pings all configured services. Reports state by using configured reporter
        /// </summary>
        public void Monitor()
        {
            //Run hearth beating in sequence (for the first version)            
            while(true)
            {
                foreach (var service in _config.Endpoints)
                {
                    //Ping the endpoint (GET request)
                    var beat = pingEndpoint(service);
                    _reporter.Report(beat);
                }

                //Configurable ping interval
                Thread.Sleep(_config.PingInterval);
            }
        }

        public bool doRequest(string endpoint, HttpMethod method, HttpStatusCode successStatus)
        {
            var request = WebRequest.CreateHttp(endpoint);
            request.Method = method.ToString();
            var response = (HttpWebResponse)request.GetResponse();

            return response.StatusCode == successStatus;
        }
    }
}
