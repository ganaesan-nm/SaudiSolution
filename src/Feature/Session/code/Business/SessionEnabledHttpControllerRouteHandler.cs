using System;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;

namespace SaudiA.Foundation.Session.Business
{
    public class SessionEnabledHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var routeData = requestContext.RouteData;
            if (routeData.DataTokens["__ReadOnlySession"] == null)
            {
                throw new InvalidOperationException("This route is not compatible with session.");
            }

            var readOnlySession = (bool)routeData.DataTokens["__ReadOnlySession"];
            if (readOnlySession)
            {
                return new ReadOnlySessionHttpControllerHandler(routeData);
            }
            return new SessionRequiredHttpControllerHandler(routeData);
        }
    }
}