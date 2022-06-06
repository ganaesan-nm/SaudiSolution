using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaudiA.WebPortal.Foundation.ServiceClient.Models
{
    public class ServiceRequest
    {
        public string BaseUrl { get; set; }

        public string Endpoint { get; set; }

        public string RequestData { get; set; }

        public RequestHeaders RequestHeaders { get; set; }
    }
}
