using System.Collections.Generic;
using WebPing.Reporters;

namespace WebPing
{
    public class WebPingConfiguration
    {
        public ICollection<string> ServiceNames { get; set; }

        public IDictionary<string, string> ServiceMap { get; set; }

        public DefaultReporters Reporter { get; set; }
    }
}
