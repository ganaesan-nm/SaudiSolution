using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaudiA.WebPortal.Foundation.ServiceClient.Models
{
    public class RequestHeaders
    {
        public string ContentType { get; set; }
        public string AcceptLanguage { get; set; }
        public string XRequestedWith { get; set; }
        public string XCSRFToken { get; set; }
        public string Authorization { get; set; }
    }
}
