using System;

namespace WebPing
{
    public struct HearthBeat: IEquatable<HearthBeat>
    {
        public HearthBeat(ServiceEndpoint service, long responseTime, bool up)
            : this (service.Name, service.Url, responseTime, up) { }

        public HearthBeat(string serviceName, string serviceEndpoint, long responseTime, bool up)
            : this()
        {
            ServiceName = serviceName;
            ServiceEndpoint = serviceEndpoint;
            ResponseTime = responseTime;
            Up = up;
        }

        /// <summary>
        /// Name of the service
        /// </summary>
        public string ServiceName { get; }
        /// <summary>
        /// Http endpoint of the service
        /// </summary>
        public string ServiceEndpoint { get; }
        /// <summary>
        /// Response time in milliseconds
        /// </summary>
        public long ResponseTime { get; }
        /// <summary>
        /// Is service up or down?
        /// </summary>
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
