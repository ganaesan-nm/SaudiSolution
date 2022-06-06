using System;
using System.Dynamic;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using SaudiA.Foundation.Session.Business;
using Sitecore.Diagnostics;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace SaudiA.Foundation.Session.Controllers
{
    [EnableCors("*", "*", "*")]
    public class SessionController : ApiController
    {
        private readonly ISessionManager _sessionManager = new SessionManager();

        [HttpGet]
        [Route("api/session/getsessionbykey/{key}")]
        public dynamic GetSessionByKey(string key)
        {
            var model = _sessionManager.Get<dynamic>(key);
            return model;
        }

        [HttpPost]
        [Route("api/session/savesession")]
        public dynamic SaveSession([FromBody] JObject jsonModel)
        {
            try
            {
                if (jsonModel != null && jsonModel.First != null)
                {
                    _sessionManager.Save(((JProperty)jsonModel.First).Name, jsonModel);

                    return jsonModel;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in controller SessionController", ex);
            }
            return jsonModel;
        }
    }
}