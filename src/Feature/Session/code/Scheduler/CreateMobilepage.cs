using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SaudiA.Foundation.Session.Business;
using SaudiA.Foundation.Session.Model;
using Sitecore.Data;
using Sitecore.Configuration;

namespace SaudiA.Foundation.Session.Scheduler
{
    public class CreateMobilepage
    {
        private  Database masterDb = Factory.GetDatabase("master");
        private Database coreDB = Factory.GetDatabase("core");
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside create Messsage ", this);

                var mobileJsonModel=GetDataFromMobilJSON();



                GetSitecoreItems(items, mobileJsonModel);

                Sitecore.Diagnostics.Log.Info("Create mobile job completed successfully", this);


            }
            catch (Exception ex)
            {

                Sitecore.Diagnostics.Log.Error("Error in scheduler", ex);
            }
        }

        private MobileJSONModel GetDataFromMobilJSON()
        {
            MobileJSONModel mobileSONModel = new MobileJSONModel();

            using (StreamReader r = new StreamReader("C:\\temp\\mobilejson\\screen_layout_hotel.json"))
            {
                string json = r.ReadToEnd();
                JObject mobileObject = JObject.Parse(json);

                if (mobileObject == null)
                {
                    return null;
                }

                foreach (var x in mobileObject)
                {
                    string name = x.Key;
                    JToken value = x.Value;

                    var placeholderList = value["placeholders"];
                    List<string> placeholderListToCheckInSitecore = new List<string>();
                    List<SitecoreModelItem> componentListToCheckInSitecore = new List<SitecoreModelItem>();

                    if (placeholderList != null)
                    {
                        foreach (var placeholder in placeholderList)
                        {
                            placeholderListToCheckInSitecore.Add(((JProperty)placeholder).Name);// Placeholder Name

                            foreach (var componentListFromJSON in placeholder)
                            {
                                foreach (var componentDetailFromJSON in componentListFromJSON)
                                {
                                    var componentobject = ((JValue)componentDetailFromJSON["componentName"]);

                                    if (componentobject == null)
                                    {
                                        Sitecore.Diagnostics.Log.Error("component object is null for componentDetailFromJSON" + componentDetailFromJSON, this); 
                                        return null;
                                    }

                                    var componentModel = new SitecoreModelItem();
                                    componentModel.ItemName = componentobject.Value.ToString();
                                   // componentModel.FieldValue = componentobject.Value.ToString();
                                    var fieldsList = componentDetailFromJSON["fields"];
                                    if (fieldsList == null || fieldsList.Count()==0)
                                    {
                                        Sitecore.Diagnostics.Log.Error("fieldsList is null for Component" + componentobject.Value.ToString(), this);
                                        return null;
                                    }
                                    componentModel.FieldList = new List<FieldWithType>();
                                    bool isFieldArray = false;
                                    foreach (var fieldvalue in fieldsList)
                                    {
                                        foreach (var field in fieldvalue)
                                        {
                                            if (field.GetType().Name.ToLower() == "jarray")
                                            {
                                                isFieldArray = true;
                                                //For array picking only the first element for template and also need to create a list field
                                                foreach (var valuefromArray in field.First)
                                                {
                                                    GetFieldValue(componentModel, valuefromArray);
                                                }
                                            }
                                        }
                                        if (isFieldArray)
                                        {
                                            isFieldArray = false;
                                            componentModel.TreeListField = new List<FieldWithType>();
                                            GetFieldValue(componentModel, fieldvalue, false);

                                        }
                                        else
                                        {
                                            GetFieldValue(componentModel, fieldvalue);
                                        }
                                    }
                                    componentListToCheckInSitecore.Add(componentModel);
                                }
                            }
                        }
                    }
                    mobileSONModel.ComponentListWithTemplate = new List<SitecoreModelItem>();
                    mobileSONModel.ComponentListWithTemplate = componentListToCheckInSitecore;
                    mobileSONModel.PlaceholderList = placeholderListToCheckInSitecore;
                }
            }
            return mobileSONModel;
        }

        private void GetSitecoreItems(Item[] items, MobileJSONModel mobileSONModel)
        {
            if (items == null)
            {
                Sitecore.Diagnostics.Log.Error("CreateMobilePage Scheudler, rendering & placeholder item is null", this);
            }
            CheckforPlaceholder(items, mobileSONModel);

            CheckForComponentWithTemplate(items, mobileSONModel);


          //  CheckForTempalte(items);


        }

        private void CheckForTempalte(Item[] items)
        {
            var templatePath = items[3];
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                var placeholderTemplate = masterDb.GetTemplate("{AB86861A-6030-46C5-B394-E8F99E8B87DB}");
                var newTempalteItem = masterDb.Templates.CreateTemplate("FlightSearch", templatePath);

                var sectionName = newTempalteItem.AddSection("Data", false);
                sectionName.AddField("Title");
                var fieldItem = newTempalteItem.GetField("Title");
                Sitecore.Data.Items.Item textFieldType = coreDB.GetItem("/sitecore/system/field types/simple types/single-line text");
                fieldItem.BeginEdit();
                fieldItem.Type = textFieldType.Name;

                fieldItem.Title = "Title";
                fieldItem.InnerItem.Fields[Sitecore.TemplateFieldIDs.Description].Value = "Title";
                fieldItem.EndEdit();
            }

        }

        private void CheckforPlaceholder(Item[] items, MobileJSONModel mobileSONModel)
        {
            var placeholderItemPath = items[0];
            var palceholderItemDestinatoinPath = items[1];
            var placholderItemList = placeholderItemPath.Axes.GetDescendants().Where(x => (x.TemplateID.ToString().ReplaceEmptyBracesAndHyphen().ToLower()
                                   .Equals((Templates.MobileSettings.PlaceholderTemplateId).ToLower()))).ToList();
            List<String> placholderItemListFromSitecore = new List<String>();
            foreach (var placholderItem in placholderItemList)
            {
                var placeholderName = placholderItem.Fields[Templates.MobileSettings.PlaceholderFieldId].ToString();
                placholderItemListFromSitecore.Add(placeholderName);
            }
            foreach (var placeholderFromMObile in mobileSONModel.PlaceholderList)
            {
                if (!placholderItemListFromSitecore.Contains(placeholderFromMObile))
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        var placeholderTemplate = masterDb.GetTemplate(Templates.MobileSettings.PlaceholderTemplateIdWithHyphen);

                        var newItem = palceholderItemDestinatoinPath.Add(placeholderFromMObile, placeholderTemplate);
                        newItem.Versions.AddVersion();
                        using (new EditContext(newItem))
                        {
                            newItem[Templates.MobileSettings.PlaceholderFieldId] = placeholderFromMObile;
                        }
                    }
                }
            }
        }

        private void CheckForComponentWithTemplate(Item[] items, MobileJSONModel mobileSONModel)
        {
            var componentItemPath = items[2];
            var componentDestinationpath = items[3];

            var mobileTemplateDesinationPath = items[4];


            var ComponentItemList = componentItemPath.Axes.GetDescendants().Where(x => (x.TemplateID.ToString().ReplaceEmptyBracesAndHyphen().ToLower()
                                   .Equals((Templates.MobileSettings.ComponentTemplateId).ToLower()))).ToList();
            List<String> componentListFromSitecore = new List<String>();
            foreach (var componentItem in ComponentItemList)
            {
                var componentName = componentItem.Fields[Templates.MobileSettings.ComponentNameFieldId].ToString();
                componentListFromSitecore.Add(componentName);
            }

            var templateList = mobileTemplateDesinationPath.Axes.GetDescendants().Where(x => (x.TemplateID.ToString().ReplaceEmptyBracesAndHyphen().ToLower()
                                  .Equals((Templates.MobileSettings.ModelTemplateId).ToLower()))).ToList();
            List<String> templateListFromSitecore = new List<String>();
            foreach (var templateItem in templateList)
            {               
                templateListFromSitecore.Add(templateItem.Name);
            }

            foreach (var componentItem in mobileSONModel.ComponentListWithTemplate.GroupBy(x=>x.ItemName))
            {
                var componentItemFromMobile = componentItem.First();
                if (!componentListFromSitecore.Contains(componentItemFromMobile.ItemName))
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        var componenTeamplate = masterDb.GetTemplate(Templates.MobileSettings.ComponentTemplateIdWithHyphen);
                        var newItem = componentDestinationpath.Add(componentItemFromMobile.ItemName, componenTeamplate);
                        newItem.Versions.AddVersion();
                        using (new EditContext(newItem))
                        {
                            newItem[Templates.MobileSettings.ComponentNameFieldId] = componentItemFromMobile.ItemName;
                            newItem[Templates.MobileSettings.ComponentNameDataSource] = "./Page Components";
                        }
                    }
                }

                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    
                    if (componentItemFromMobile.FieldList != null)
                    {
                        TemplateItem tempalteItem = null;
                        if (!templateListFromSitecore.Contains(componentItemFromMobile.ItemName))
                        {
                            tempalteItem = masterDb.Templates.CreateTemplate(componentItemFromMobile.ItemName, mobileTemplateDesinationPath);
                            CreateFieldValues(componentItemFromMobile.FieldList, tempalteItem);

                        }
                        else
                        {
                            tempalteItem = masterDb.GetItem(mobileTemplateDesinationPath.Paths.Path + "//" + componentItemFromMobile.ItemName);
                            CreateFieldValues(componentItemFromMobile.FieldList, tempalteItem);
                        }
                       
                    }
                    if (componentItemFromMobile.TreeListField != null)
                    {
                        TemplateItem tempalteItem = null;
                        if (!templateListFromSitecore.Contains(componentItemFromMobile.ItemName+"List"))
                        {
                            tempalteItem = masterDb.Templates.CreateTemplate(componentItemFromMobile.ItemName+"List", mobileTemplateDesinationPath);

                            CreateFieldValues(componentItemFromMobile.TreeListField, tempalteItem);

                            //foreach (var field in componentItemFromMobile.TreeListField)
                            //{
                            //    var sectionName = tempalteItem.AddSection("Data", false);
                            //    var fieldItem = sectionName.AddField(field.FieldName);
                            //    Sitecore.Data.Items.Item textFieldType = coreDB.GetItem(GetSitecoreFieldType(field.Type));
                            //    fieldItem.BeginEdit();
                            //    fieldItem.Type = textFieldType.Name;
                            //    fieldItem.Title = field.FieldName;
                            //    fieldItem.InnerItem.Fields[Sitecore.TemplateFieldIDs.Description].Value = field.FieldName;
                            //    fieldItem.EndEdit();
                            //}

                        }
                        else
                        {
                            tempalteItem = masterDb.GetItem(mobileTemplateDesinationPath.Paths.Path + "//" + componentItemFromMobile.ItemName+ "List");
                            CreateFieldValues(componentItemFromMobile.TreeListField, tempalteItem);
                        }

                    }

                }

            }
        }

        private void CreateFieldValues(List<FieldWithType> fieldList, TemplateItem tempalteItem)
        {
            foreach (var field in fieldList)
            {
                var sectionItem = tempalteItem.GetSection("Data");
                if (sectionItem == null)
                {
                    sectionItem = tempalteItem.AddSection("Data", false);
                }

                var fieldItem = sectionItem.GetField(field.FieldName);
                if (fieldItem == null)
                {
                    fieldItem = sectionItem.AddField(field.FieldName);
                    Sitecore.Data.Items.Item textFieldType = coreDB.GetItem(GetSitecoreFieldType(field.Type));
                    fieldItem.BeginEdit();
                    fieldItem.Type = textFieldType.Name;
                    fieldItem.Title = field.FieldName;
                    if (field.Type == FieldType.MultiList)
                    {
                        fieldItem.Source = "query:.//*";
                    }
                    fieldItem.InnerItem.Fields[Sitecore.TemplateFieldIDs.Description].Value = field.FieldName;
                    fieldItem.EndEdit();
                }
                
            }
            
        }

        //private bool IsShared(FieldType fieldType)
        //{
        //    if ((fieldType == FieldType.Boolean || fieldType == FieldType.Image || fieldType  == FieldType.Generallink))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        private string GetSitecoreFieldType(FieldType fieldType)
        {
            string sitecoreFieldType = "/sitecore/system/field types/simple types/single-line text";
            switch (fieldType)
            {
                case FieldType.SingleLine:
                    sitecoreFieldType = "/sitecore/system/field types/simple types/single-line text";
                    break;
                case FieldType.MultiLine:
                    sitecoreFieldType = "/sitecore/system/Field types/Simple Types/Multi-Line Text";
                    break;
                case FieldType.Image:
                    sitecoreFieldType = "/sitecore/system/Field types/Simple Types/Image";
                    break;
                case FieldType.Generallink:
                    sitecoreFieldType = "/sitecore/system/Field types/Link Types/General Link";
                    break;
                case FieldType.Boolean:
                    sitecoreFieldType = "/sitecore/system/Field types/Simple Types/Checkbox";
                    break;
                case FieldType.MultiList:
                    sitecoreFieldType = "/sitecore/system/Field types/List Types/TreelistEx";
                    break;
                default:
                    sitecoreFieldType = "/sitecore/system/field types/simple types/single-line text";
                    break;
            }
            return sitecoreFieldType;
        }

        private  void GetFieldValue(SitecoreModelItem componentModel, JToken valuefromArray, bool isFieldList = true)
        {
            FieldWithType fieldWithType = new FieldWithType();
            fieldWithType.FieldName = ((Newtonsoft.Json.Linq.JProperty)valuefromArray).Name;

            if (isFieldList)
            {
                fieldWithType.Type = GetFieldType(((Newtonsoft.Json.Linq.JProperty)valuefromArray).Value, fieldWithType.FieldName);
                componentModel.FieldList.Add(fieldWithType);
            }
            else
            {
                fieldWithType.Type = FieldType.MultiList;
                componentModel.TreeListField.Add(fieldWithType);
            }
        }

        private  FieldType GetFieldType(JToken value, string fieldName)
        {
            if (value != null)
            {
                if (fieldName.ToString().ToLower().Contains("is"))
                {
                    return FieldType.Boolean;
                }
                else if (value.ToString().Contains("src"))
                {
                    return FieldType.Image;
                }
                else if (value.ToString().Contains("url"))
                {
                    return FieldType.Generallink;
                }

            }
            return FieldType.SingleLine;

        }

    }
}