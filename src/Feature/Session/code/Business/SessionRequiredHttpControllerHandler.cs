using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace SaudiA.Foundation.Session.Business
{
    public class SessionRequiredHttpControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionRequiredHttpControllerHandler(RouteData routeData)
            : base(routeData)
        {
        }
    }
}