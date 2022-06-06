using Sitecore.JavaScriptServices.Configuration;
using Sitecore.LayoutService.ItemRendering.Pipelines.GetLayoutServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaudiA.Foundation.Session.LayoutServiceContext
{
    public class SaudiaContextExtension : Sitecore.JavaScriptServices.ViewEngine.LayoutService.Pipelines.GetLayoutServiceContext.JssGetLayoutServiceContextProcessor
    {
        public SaudiaContextExtension(IConfigurationResolver configurationResolver) : base(configurationResolver)
        {
        }

        protected override void DoProcess(GetLayoutServiceContextArgs args, AppConfiguration application)
        {
            args.ContextData.Add("securityInfo", new
            {
                isAnonymous = !Sitecore.Context.User.IsAuthenticated
            });
        }

    }
}