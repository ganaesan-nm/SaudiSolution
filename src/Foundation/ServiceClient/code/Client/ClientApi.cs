using SaudiA.WebPortal.Foundation.ServiceClient.Constant;
using SaudiA.WebPortal.Foundation.ServiceClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SaudiA.WebPortal.Foundation.ServiceClient.Client
{
    public class ClientApi
    {

        public List<Cookie> cookieContainer = null;

        #region -- Methods --


        /// <summary>
        /// Get odata api responses
        /// </summary>
        /// <returns>apiResponse</returns>
        public HttpResponseMessage GetOdataApiResponse(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, null))
                {
                    httpResponseMessage = client.GetAsync(serviceRequest.Endpoint).Result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Get odata api responses
        /// </summary>
        /// <returns>apiResponse</returns>
        public HttpResponseMessage GetOdataApiResponseByPassingCookieContainer(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {
                
                CookieContainer _cookieJar = new CookieContainer();
                if (cookieContainer != null)
                {
                    foreach (Cookie cookie in cookieContainer)
                    {
                        Cookie seccookie3 = new Cookie()
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain
                        };
                        _cookieJar.Add(seccookie3);

                        Log.Info("cookie cookie.Name:cookie.Value " + serviceRequest.Endpoint + ":"+ cookie.Name+":"+ cookie.Value, this);
                    }
                }
                
                using (var handler = new HttpClientHandler { CookieContainer = _cookieJar })
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, handler))
                {
                    client.Timeout = new TimeSpan(0,2,50);
                    httpResponseMessage = client.GetAsync(serviceRequest.Endpoint).Result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Post odata api responses
        /// </summary>
        /// <returns>apiResponse</returns>
        public HttpResponseMessage PostOdataApiResponseByPassingCookieContainer(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {                
               
                CookieContainer _cookieJar = new CookieContainer();
                if (cookieContainer != null)
                {
                    foreach (Cookie cookie in cookieContainer)
                    {
                        Cookie seccookie3 = new Cookie()
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain
                        };
                        _cookieJar.Add(seccookie3);
                    }
                }                

                using (var handler = new HttpClientHandler { CookieContainer = _cookieJar })
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, handler))
                {
                    httpResponseMessage = client.PostAsync(serviceRequest.Endpoint, null).Result;
                }               
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Post odata api with cookie container response
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns>apiResponse</returns>
        public HttpResponseMessage PostODataApiWithCookieResponse(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {

                
                
                CookieContainer _cookieJar = new CookieContainer();
                if (cookieContainer != null)
                {
                    foreach (Cookie cookie in cookieContainer)
                    {
                        Cookie seccookie3 = new Cookie()
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain
                        };
                        Log.Info("cookie cookie.Name:cookie.Value " + cookie.Name + ":" + cookie.Value, this);
                        _cookieJar.Add(seccookie3);
                    }
                }

                using (var handler = new HttpClientHandler { CookieContainer = _cookieJar })
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, handler))
                {
                    if (!string.IsNullOrWhiteSpace(serviceRequest.RequestData))
                    {
                        HttpContent stringContent = new StringContent(serviceRequest.RequestData, Encoding.UTF8, ServiceConstant.APPLICATIONJSON);
                        httpResponseMessage = client.PostAsync(serviceRequest.Endpoint, stringContent).Result;
                    }
                    else
                    {
                        httpResponseMessage = client.PostAsync(serviceRequest.Endpoint, null).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }


        /// <summary>
        /// Post odata api response
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns>apiResponse</returns>
        public HttpResponseMessage PostODataApiResponse(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, null))
                {
                    if (!string.IsNullOrWhiteSpace(serviceRequest.RequestData))
                    {
                        HttpContent stringContent = new StringContent(serviceRequest.RequestData, Encoding.UTF8, ServiceConstant.APPLICATIONJSON);
                        httpResponseMessage = client.PostAsync(serviceRequest.Endpoint, stringContent).Result;
                    }
                    else
                    {
                        httpResponseMessage = client.PostAsync(serviceRequest.Endpoint, null).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Put http service client
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="endpoint"></param>
        /// <param name="request"></param>
        /// <param name="requestHeaders"></param>
        /// <returns>ServiceResponse</returns>
        public HttpResponseMessage PutODataApiResponse(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {

                
                
                CookieContainer _cookieJar = new CookieContainer();
                if (cookieContainer != null)
                {
                    foreach (Cookie cookie in cookieContainer)
                    {
                        Cookie seccookie3 = new Cookie()
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain
                        };
                        _cookieJar.Add(seccookie3);
                    }
                }

                using (var handler = new HttpClientHandler { CookieContainer = _cookieJar })
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, handler))
                {
                    if (!string.IsNullOrWhiteSpace(serviceRequest.RequestData))
                    {
                        HttpContent stringContent = new StringContent(serviceRequest.RequestData, Encoding.UTF8, serviceRequest.RequestHeaders.ContentType);//It should application-json
                        httpResponseMessage = client.PutAsync(serviceRequest.Endpoint, stringContent).Result;
                    }
                    else
                    {
                        httpResponseMessage = client.PutAsync(serviceRequest.Endpoint, null).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Delete http service client
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="endpoint"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        public HttpResponseMessage DeleteODataApiResponse(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, null))
                {
                    httpResponseMessage = client.DeleteAsync(serviceRequest.Endpoint).Result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }

        /// <summary>
        /// Delete http service client api with cookie container response
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns>apiResponse</returns>
        public HttpResponseMessage DeleteODataApiWithCookieResponse(ServiceRequest serviceRequest)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            try
            {

                
                
                CookieContainer _cookieJar = new CookieContainer();
                if (cookieContainer != null)
                {
                    foreach (Cookie cookie in cookieContainer)
                    {
                        Cookie seccookie3 = new Cookie()
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain
                        };
                        _cookieJar.Add(seccookie3);
                    }
                }

                using (var handler = new HttpClientHandler { CookieContainer = _cookieJar })
                using (HttpClient client = SetHTTPClientRequest(serviceRequest, handler))
                {
                    httpResponseMessage = client.DeleteAsync(serviceRequest.Endpoint).Result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                httpResponseMessage.ReasonPhrase = ex.Message;
            }

            return httpResponseMessage;
        }
        #endregion

        #region -- Helper methods --       

        public HttpClient SetHTTPClientRequest(ServiceRequest serviceRequest, HttpClientHandler httpClientHandler)
        {
            HttpClient client = httpClientHandler != null ? new HttpClient(httpClientHandler) : new HttpClient();

            try
            {
                string contentType = string.Empty;

                if (serviceRequest.RequestHeaders.ContentType.Contains("charset=utf-8"))
                {
                    contentType = serviceRequest.RequestHeaders.ContentType.Split(';')[0].ToString();
                }
                else
                    contentType = serviceRequest.RequestHeaders.ContentType;

                if (!string.IsNullOrWhiteSpace(contentType))
                    client.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue(contentType));

                if (!string.IsNullOrWhiteSpace(serviceRequest.RequestHeaders.AcceptLanguage))
                    client.DefaultRequestHeaders
                        .Add(ServiceConstant.REQUESTHEADERS_KEY_ACCEPTLANGUAGE, serviceRequest.RequestHeaders.AcceptLanguage);

                if (!string.IsNullOrWhiteSpace(serviceRequest.RequestHeaders.XRequestedWith))
                    client.DefaultRequestHeaders
                        .Add(ServiceConstant.REQUESTHEADERS_KEY_XREQUESTEDWITH, serviceRequest.RequestHeaders.XRequestedWith);

                if (!string.IsNullOrWhiteSpace(serviceRequest.RequestHeaders.XCSRFToken))
                    client.DefaultRequestHeaders
                        .Add(ServiceConstant.REQUESTHEADERS_KEY_XCSRFTOKEN, serviceRequest.RequestHeaders.XCSRFToken);

                if (!string.IsNullOrWhiteSpace(serviceRequest.RequestHeaders.Authorization))
                    client.DefaultRequestHeaders.Add(ServiceConstant.REQUESTHEADERS_KEY_AUTHORIZATION, serviceRequest.RequestHeaders.Authorization);

                client.BaseAddress = new Uri(serviceRequest.BaseUrl);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                client = null;
            }

            return client;
        }

        #endregion
    }
}