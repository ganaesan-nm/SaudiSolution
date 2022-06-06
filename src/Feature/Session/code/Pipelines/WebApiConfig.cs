using System.Web.Http;
using SaudiA.Foundation.Session.Business;
using Sitecore.Pipelines;

namespace SaudiA.Foundation.Session.Pipelines
{
    public class WebApiConfig
    {
        public void Process(PipelineArgs args)
        {
            Register(GlobalConfiguration.Configuration);
        }

        private static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpSessionRoute(
                name: "GetMessageApi",
                routeTemplate: "apisession/getmessage/{id}",
                defaults: new { controller = "GetMessage" },
                readOnlySession: true
            );

            config.Routes.MapHttpSessionRoute(
                name: "SetMessageApi",
                routeTemplate: "apisession/setmessage",
                defaults: new { controller = "SetMessage" },
                readOnlySession: false
            );

            config.Routes.MapHttpSessionRoute(
               name: "GetCookieValue",
               routeTemplate: "apisession/getcookievalue",
               defaults: new { controller = "GetMessage" },
               readOnlySession: true
           );
        }
    }
}