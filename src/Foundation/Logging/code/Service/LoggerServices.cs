using SaudiA.WebPortal.Foundation.Logging.IService;
using log4net;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SaudiA.WebPortal.Foundation.Logging.Service
{
    public abstract class LoggerServices : ILoggerServices
    {
        public readonly ILog _log;
        private const string AspNetSessionKey = "ASP.NET_SessionId";

        protected LoggerServices(string logger)
        {
            _log = LoggerFactory.GetLogger(logger);
        }

        void ILoggerServices.Debug(string message)
        {
            _log.Debug(PrependTraceId(message));
        }

        void ILoggerServices.Debug(string message, Exception ex)
        {
            _log.Debug(message, ex);
        }

        void ILoggerServices.Info(string message)
        {
            _log.Info(PrependTraceId(message));
        }

        void ILoggerServices.Info(string message, Exception ex)
        {
            _log.Info(message, ex);
        }

        void ILoggerServices.Warn(string message)
        {
            _log.Warn(PrependTraceId(message));
        }

        void ILoggerServices.Warn(string message, Exception ex)
        {
            _log.Warn(PrependTraceId(message), ex);
        }

        void ILoggerServices.Error(string message)
        {
            _log.Error(PrependTraceId(message));
        }

        void ILoggerServices.Error(string message, Exception ex)
        {
            _log.Error(PrependTraceId(message), ex);
        }

        void ILoggerServices.Fatal(string message)
        {
            _log.Fatal(PrependTraceId(message));
        }

        void ILoggerServices.Fatal(string message, Exception ex)
        {
            _log.Fatal(message, ex);
        }

        private string PrependTraceId(string message)
        {
            var builder = new StringBuilder();
            if (HttpContext.Current?.Request?.Cookies?[AspNetSessionKey] != null)
            {
                var aspSessionId = HttpContext.Current.Request.Cookies[AspNetSessionKey].Value;
                if (!string.IsNullOrWhiteSpace(aspSessionId))
                {
                    builder.Append("AspNetSessionID:");
                    builder.Append(aspSessionId);
                    builder.Append(" ");
                }
            }
            builder.Append(message);
            return builder.ToString();
        }
    }
}