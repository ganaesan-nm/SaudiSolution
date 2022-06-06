
using SaudiA.WebPortal.Foundation.ServiceClient.Client;
using SaudiA.WebPortal.Foundation.ServiceClient.Constant;
using SaudiA.WebPortal.Foundation.ServiceClient.Models;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaudiA.WebPortal.Foundation.ServiceClient
{
    public class ClientCommon
    {
        #region -- Properties --

        private readonly ClientApi clientApi;
        Sitecore.Data.ID eserviceMappingSettings;
        #endregion

        #region -- Constructors --        

        public ClientCommon()
        {
            clientApi = new ClientApi();
            eserviceMappingSettings = new Sitecore.Data.ID(ServiceConstant.SITECORE_ID_ESERVICEMAPPING);
        }

        #endregion

        #region -- Methods -- 

        
       

        #region 3. Convert to date time string.        

        /// <summary>
        /// Convert to date time string
        /// </summary>
        /// <param name="date">date</param>
        /// <returns>dateResp</returns>
        public string ConvertToDateTimeString(string date)
        {
            string dateResp = date;

            if (!string.IsNullOrWhiteSpace(date))
                dateResp = new DateTime(1970, 1, 1, 3, 0, 0).AddMilliseconds(double.Parse(Regex.Match(date, @"(\d+)").Value)).ToString(ServiceConstant.DATEFORMAT);

            return dateResp;
        }

        #endregion

        #region g. GetKeyValuePairList.        

        /// <summary>
        /// GetKeyValuePairList
        /// </summary>
        /// <param name="date">date</param>
        /// <returns>dateResp</returns>
        public List<KeyValuePair<string, string>> GetKeyValuePairList(string data)
        {
            string[] keyValuePairArray = data.ToString().Split('&');
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (keyValuePairArray != null && keyValuePairArray.Length > 0 && !String.IsNullOrEmpty(keyValuePairArray[0]))
            {
                foreach (var item in keyValuePairArray)
                {
                    string[] keyValues = item.Split('=');
                    var key = keyValues[0];
                    var value = Uri.UnescapeDataString(keyValues[1]);
                    result.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            return result;
        }
        #endregion


        #region 4. Get request headers.        

        /// <summary>
        /// Get request headers
        /// </summary>
        /// <param name="httpRequestBase">httpRequestBase</param>
        /// <returns>Request headers</returns>
        public RequestHeaders GetRequestHeaders(HttpRequestBase httpRequestBase)
        {
            RequestHeaders requestHeaders = new RequestHeaders();

            if (httpRequestBase != null)
            {
                requestHeaders.ContentType = httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_CONTENTTYPE] != null ? httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_CONTENTTYPE].Trim().ToString() : string.Empty;
                requestHeaders.AcceptLanguage = httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_ACCEPTLANGUAGE] != null ? httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_ACCEPTLANGUAGE].Trim().ToString() : string.Empty;
                requestHeaders.XRequestedWith = httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_XREQUESTEDWITH] != null ? httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_XREQUESTEDWITH].Trim().ToString() : string.Empty;
                requestHeaders.XCSRFToken = httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_XCSRFTOKEN] != null ? httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_XCSRFTOKEN].Trim().ToString() : string.Empty;
                requestHeaders.Authorization = httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_AUTHORIZATION] != null ? httpRequestBase.Headers[ServiceConstant.REQUESTHEADERS_KEY_AUTHORIZATION].Trim().ToString() : string.Empty;
            }

            return requestHeaders;
        }

        #endregion

              

        #region 10. Error Key mapping     
        /// <summary>
        /// Error key mapping
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public string GetErrorMessageByCode(string errorCode, string fieldName = "")
        {
            string errorMessage = string.Empty;
            Sitecore.Data.ID errorMappingSettings = new Sitecore.Data.ID(ServiceConstant.SITECORE_ID_ERRORMAPPING);
            if (!errorMappingSettings.IsNull)
            {
                Item sItem = Sitecore.Context.Database.GetItem(errorMappingSettings);
                if (sItem == null)
                    return errorMessage;

                string errorMappingFieldName = string.Empty;
                if (fieldName != string.Empty)
                {
                    errorMappingFieldName = fieldName;
                }
                else
                {
                    errorMappingFieldName = ServiceConstant.SITECORE_FIELD_DEFAULTERRORMAPPING;
                }
                var errorMappingFieldValue = sItem.Fields[errorMappingFieldName]?.Value;
                if (!String.IsNullOrEmpty(errorMappingFieldValue))
                {
                    var errorMappingList = GetKeyValuePairList(errorMappingFieldValue);
                    if (errorMappingList != null)
                    {
                        var errorMeesageValue = errorMappingList.Find(s => s.Key == errorCode).Value;
                        if (errorMeesageValue != null)
                        {
                            errorMessage = errorMeesageValue;
                        }
                    }
                }
            }
            return errorMessage;

        }
        #endregion

        #region 10. EService Key mapping     
        /// <summary>
        /// EService key mapping
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public string GetEServiceMappingsByCode(string mappingCode, string fieldName = "")
        {
            string mappingText = mappingCode;
            Sitecore.Data.ID eserviceMappingSettings = new Sitecore.Data.ID(ServiceConstant.SITECORE_ID_ESERVICEMAPPING);
            if (!eserviceMappingSettings.IsNull)
            {
                Item sItem = Sitecore.Context.Database.GetItem(eserviceMappingSettings);
                if (sItem == null)
                    return mappingText;

                string eserviceMappingFieldName = string.Empty;
                if (fieldName != string.Empty)
                {
                    eserviceMappingFieldName = fieldName;
                }
                else
                {
                    eserviceMappingFieldName = ServiceConstant.SITECORE_FIELD_PROCESSTYPE;
                }
                var eserviceMappingFieldValue = sItem.Fields[eserviceMappingFieldName]?.Value;
                if (!String.IsNullOrEmpty(eserviceMappingFieldValue))
                {
                    var eserviceMappingList = GetKeyValuePairList(eserviceMappingFieldValue);
                    if (eserviceMappingList != null)
                    {
                        mappingCode = mappingCode.Replace(" ", "").Trim();
                        var mappedValue = eserviceMappingList.Find(s => s.Key == mappingCode).Value;
                        if (mappedValue != null)
                        {
                            mappingText = mappedValue;
                        }
                    }
                }

            }
            return mappingText;

        }
        #endregion

        #region 10.a. EService mapping list
        /// <summary>
        /// EService key mapping
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetEServiceMappingsList(string fieldName = "")
        {
            var _eserviceMappingSettings = eserviceMappingSettings;
            var eserviceMappingList = new List<KeyValuePair<string, string>>();
            if (!eserviceMappingSettings.IsNull)
            {
                Item sItem = Sitecore.Context.Database.GetItem(_eserviceMappingSettings);
                if (sItem == null)
                    return new List<KeyValuePair<string, string>>();

                string eserviceMappingFieldName = string.Empty;
                if (fieldName != string.Empty)
                {
                    eserviceMappingFieldName = fieldName;
                }
                else
                {
                    eserviceMappingFieldName = ServiceConstant.SITECORE_FIELD_PROCESSTYPE;
                }
                var eserviceMappingFieldValue = sItem.Fields[eserviceMappingFieldName]?.Value;
                if (!String.IsNullOrEmpty(eserviceMappingFieldValue))
                {
                    eserviceMappingList = GetKeyValuePairList(eserviceMappingFieldValue);
                }
            }
            return eserviceMappingList;
        }
        #endregion

        #endregion
    }

    public sealed class EmptyStringModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}