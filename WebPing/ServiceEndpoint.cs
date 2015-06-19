using System.Net;
using System.Net.Http;

namespace WebPing
{
    public class ServiceEndpoint
    {
        /// <summary>
        /// Name of the service
        /// </summary>
        public string Name { get; set; }

        private HttpMethod _method = HttpMethod.Get;
        /// <summary>
        /// Http method to use on the endpoint
        /// </summary>
        public HttpMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }
        /// <summary>
        /// Service endpoint url. 
        /// If discovery service is used, Url's get updated using it. 
        /// Default discovery service uses value of this property.
        /// </summary>
        public string Url { get; set; }

        private HttpStatusCode _successStatus = HttpStatusCode.OK;
        /// <summary>
        /// Http status code considered as a success.
        /// </summary>
        public HttpStatusCode SuccessStatus
        {
            get { return _successStatus;  }
            set { _successStatus = value; }
        }
    }
}
