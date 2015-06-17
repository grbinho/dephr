using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPing
{
    //TODO: Struct?
    public struct HearthBeat: IEquatable<HearthBeat>
    {
        public HearthBeat(string serviceName, string serviceEndpoint, long responseTime, bool up): this()
        {
            ServiceName = serviceName;
            ServiceEndpoint = serviceEndpoint;
            ResponseTime = responseTime;
            Up = up;
        }

        public string ServiceName { get; }
        public string ServiceEndpoint { get; }
        /// <summary>
        /// Response time in milliseconds
        /// </summary>
        public long ResponseTime { get; }
        public bool Up { get; }


        //TODO: Additional information
        //TODO: How to property measure response time
        public override string ToString()
        {
            return string.Format("Service={0}, Endpoint={1}, ResponseTime={2}, Up={3}", ServiceName, ServiceEndpoint, ResponseTime,Up);
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            var other = (HearthBeat)obj;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(HearthBeat other)
        {
            return ServiceName == other.ServiceName &&
                ServiceEndpoint == other.ServiceEndpoint &&
                ResponseTime == other.ResponseTime &&
                Up == other.Up;

        }
    }
}
