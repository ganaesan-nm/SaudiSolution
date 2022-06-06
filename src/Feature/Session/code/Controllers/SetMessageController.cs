using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaudiA.Foundation.Session.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SaudiA.Foundation.Session.Controllers
{
    public class SetMessageController : ApiController
    {
        private readonly ISessionManager _sessionManager = new SessionManager();

      
        public IHttpActionResult Post([FromBody] JObject jsonInput)
        {
            try
            {
                if (jsonInput == null)
                {
                    Sitecore.Diagnostics.Log.Error("Error in SetMessageController Post JSon input is null", this);
                    return BadRequest();
                }
                var key = ((JProperty)jsonInput?.First).Name;
                if (string.IsNullOrEmpty(key))
                {
                    Sitecore.Diagnostics.Log.Error("Error in SetMessageController Post key is empty", jsonInput);
                    return BadRequest();
                }
                _sessionManager.Save<JObject>(key, jsonInput);
                return Ok();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error in SetMessageController Post", jsonInput);
                return BadRequest();
            }
        }
    }
}