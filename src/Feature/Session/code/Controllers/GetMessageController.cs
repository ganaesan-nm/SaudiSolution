using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using SaudiA.Foundation.Session.Business;

namespace SaudiA.Foundation.Session.Controllers
{
    public class GetMessageController : ApiController
    {
        private readonly ISessionManager _sessionManager = new SessionManager();

        [HttpGet]
        public IHttpActionResult Get(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    Sitecore.Diagnostics.Log.Info("GetMessageController Get for session key : " + key, this);
                    var sessionMessage = _sessionManager.Get<string>(key);
                    if (string.IsNullOrEmpty(sessionMessage))
                    {
                        Sitecore.Diagnostics.Log.Info("GetMessageController Get value is not available for key : " + key, this);
                        return Ok(string.Empty);
                    }
                    Sitecore.Diagnostics.Log.Info($"Retrieved message from Session: {sessionMessage}", this);
                    return Ok(sessionMessage);
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("Error in GetMessageController Get method key is empty or not available", this);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error in GetMessageController Get method", ex.ToString());
                return NotFound();

            }
        }

        [HttpGet]
        public IHttpActionResult GetCookieValue()
        {
            return Ok(HttpContext.Current?.Session?.SessionID);
        }
    }
}