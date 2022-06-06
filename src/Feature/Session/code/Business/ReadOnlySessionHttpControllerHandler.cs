using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace SaudiA.Foundation.Session.Business
{
    public class ReadOnlySessionHttpControllerHandler : HttpControllerHandler, IReadOnlySessionState
    {
        public ReadOnlySessionHttpControllerHandler(RouteData routeData)
            : base(routeData)
        {
        }
    }
}