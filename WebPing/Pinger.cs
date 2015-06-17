using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using WebPing.Reporters;

namespace WebPing
{
    public class Pinger
    {
        private readonly IHearthBeatReporter _reporter;

        //TODO: Decide how long to cache service descovery results. (Configuration)
        private readonly IServiceDiscovery _serviceDiscover;

        private readonly WebPingConfiguration _config;

        private IDictionary<string, string> _serviceEndpoints;


        public Pinger(WebPingConfiguration config) 
            : this(config, DefaultReporterFactory.CreateReporter(config.Reporter), new DictionaryServiceDiscovery(config.ServiceMap)) {}

        public Pinger(WebPingConfiguration config, IHearthBeatReporter reporter)
            : this(config, reporter, new DictionaryServiceDiscovery(config.ServiceMap)) {}

        public Pinger(WebPingConfiguration config, IHearthBeatReporter reporter, IServiceDiscovery serviceDiscovery)
        {
            _serviceEndpoints = new Dictionary<string, string>();
            _config = config;
            _reporter = reporter;
            _serviceDiscover = serviceDiscovery;
        }


        /// <summary>
        /// Gets endpoint for service name. If discovery timeout has passed, uses service discovery and caches result. If not returns result from cache.
        /// </summary>
        /// <param name="serviceName">Name of the service</param>
        private bool tryGetEndpoint(string serviceName, out string endpoint)
        {
            //Check service disovery timeout (TTL)

            //If TTL == 0 || not cached, call service disovery, upon success, add to cache, upon failure, return false;

            //else get from cache.

        }

        private HearthBeat pingEndpoint(HttpClient client, string serviceName)
        {
            string endpoint;

            if (!tryGetEndpoint(serviceName, out endpoint))
            {
                return new HearthBeat(serviceName, endpoint, 0, false));
            }

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var resultTask = client.GetAsync(endpoint);
            resultTask.Wait();

            stopwatch.Stop();

            var result = resultTask.Result;
            return new HearthBeat(serviceName, endpoint, stopwatch.ElapsedMilliseconds, result.IsSuccessStatusCode);
        }

        /// <summary>
        /// Blocks and pings all configured services. Reports state by using configured reporter
        /// </summary>
        public void Monitor()
        {
            var client = new HttpClient();
            //Run hearth beating in sequence for the first version


            while(true)
            {
                foreach (var service in _config.ServiceNames)
                {
                    //Ping the endpoint (GET request)
                    var beat = pingEndpoint(client, service);
                    _reporter.Report(beat);
                }


                //Configurable ping interval
                Thread.Sleep(5000);
            }
        }
    }
}
