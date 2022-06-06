using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SaudiA.WebPortal.Foundation.ServiceClient.Models
{
    public static class SessionModel
    {
        public static CookieContainer CookieContainer { get; set; }
        public static string XCSRFToken { get; set; }
        public static string SessionID { get; set; }
 
    }
}