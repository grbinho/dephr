using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPing
{
    //TODO: Struct?
    public class HearthBeat
    {
        public string ServiceName { get; set; }
        public string ServiceEndpoint { get; set; }
        /// <summary>
        /// Response time in milliseconds
        /// </summary>
        public int ResponseTime { get; set; }


        //TODO: Additional information
        //TODO: How to property measure response time
    }
}
