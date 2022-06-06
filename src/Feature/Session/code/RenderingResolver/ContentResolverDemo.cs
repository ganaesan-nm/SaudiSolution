using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using System.Collections.Generic;
using System.Linq;

namespace SaudiA.Foundation.Session.RenderingResolver
{
    public class ContentResolverDemo : RenderingContentsResolver
    {
        private List<Item> items = new List<Item>();

        public override object ResolveContents(Sitecore.Mvc.Presentation.Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            Item ds = GetContextItem(rendering, renderingConfig);

            var recommendedItemsFieldId = new ID("{85B950A6-FBCE-49AF-8EEB-24A13D528557}");

            if (ds.Fields.Contains(recommendedItemsFieldId) && !string.IsNullOrWhiteSpace(ds.Fields[recommendedItemsFieldId].Value))
            {
                List<string> targetItemIds = ds.Fields[recommendedItemsFieldId].Value.Split('|').ToList();
                foreach (var id in targetItemIds)
                {
                    var item = Sitecore.Context.Database.GetItem(new ID(id));
                    items.Add(item);
                }
            }

            if (!items.Any())
                return null;

            JObject jobject = new JObject()
            {
                ["items11"] = (JToken)new JArray()
            };

            List<Item> objList = items != null ? items.ToList() : null;
            if (objList == null || objList.Count == 0)
                return jobject;
            jobject["items11"] = ProcessItems(objList, rendering, renderingConfig);
            return jobject;
        }
    }
}