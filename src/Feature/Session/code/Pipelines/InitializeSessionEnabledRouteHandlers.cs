using System.Linq;
using System.Web.Routing;
using SaudiA.Foundation.Session.Business;
using Sitecore.Pipelines;

namespace SaudiA.Foundation.Session.Pipelines
{
    public class InitializeSessionEnabledRouteHandlers
    {
        public void Process(PipelineArgs args)
        {
            var routes = RouteTable.Routes.OfType<Route>().Where(r => r.DataTokens != null);
            foreach (var route in routes)
            {
                if (route.DataTokens["__ReadOnlySession"] != null)
                {
                    route.RouteHandler = new SessionEnabledHttpControllerRouteHandler();
                }
            }
        }
    }
}
