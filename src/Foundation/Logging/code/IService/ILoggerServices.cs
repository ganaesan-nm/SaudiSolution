using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaudiA.WebPortal.Foundation.Logging.IService
{
    public interface ILoggerServices
    {
        #region Debug
        void Debug(string message);
        void Debug(string message, Exception ex);
        #endregion

        #region Info
        void Info(string message);
        void Info(string message, Exception ex);
        #endregion

        #region Warn
        void Warn(string message);
        void Warn(string message, Exception ex);
        #endregion

        #region Error
        void Error(string message);
        void Error(string message, Exception ex);
        #endregion

        #region Fatal
        void Fatal(string message);
        void Fatal(string message, Exception ex);
        #endregion
    }
}
