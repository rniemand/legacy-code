using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VidPub.Web.Infrastructure.Logging
{
    public interface ILogger
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception ex);
    }
}