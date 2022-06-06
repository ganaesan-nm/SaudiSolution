using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaudiA.WebPortal.Foundation.ErrorHandling.ErrorHandler
{
    public class Page404Resolver : HttpRequestProcessor
    {
        public override void Process(Sitecore.Pipelines.HttpRequest.HttpRequestArgs args)
        {
            if (Sitecore.Context.Item != null || Sitecore.Context.Site == null || Sitecore.Context.Database == null || WebUtil.GetRawUrl().IndexOf("/sitecore") > -1)
            {
                if (Sitecore.Context.Database != null && ID.IsNullOrEmpty(LanguageManager.GetLanguageItemId(Sitecore.Context.Language, Sitecore.Context.Database)))
                {
                    SetCustomErrorPage(args);
                }
                else
                {
                    return;
                }
            }
            SetCustomErrorPage(args);
        }

        private void SetCustomErrorPage(HttpRequestArgs args)
        {
            var errorPagePath = Settings.GetSetting("CustomError.Page404Resolver");

            var item = args.GetItem(errorPagePath);
            if (Sitecore.Context.Item != null && Sitecore.Context.Item.Versions.Count == 0)
            {
                item = item.Database.GetItem(item.ID);
            }
            if (item != null)
            {
                Sitecore.Context.Item = item;
                args.HttpContext.Response.StatusCode = 200;
                args.HttpContext.Response.TrySkipIisCustomErrors = true;
            }
        }

    }
}