using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebPing
{
    public static class HttpConfigurationExtensions
    {
        public static void EnableWebPing(this HttpConfiguration config)
        {

        }

        public static void EnableWebPing(this HttpConfiguration config, IDictionary<string, string> endpoints)
        {

        }

        public static void EnableWebPing(this HttpConfiguration config, WebPingConfiguration webPingConfig)
        {

        }
    }
}
