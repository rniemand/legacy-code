using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RnCore.Logging
{
    public static class RnLoggerHelper
    {

        public static void LogException(this Exception ex)
        {
            try
            {
                RnLogger.Loggers.LogException(ex.Message);
            }
            catch (Exception)
            {
                // There is nothing that I can do here
            }
        }


        public static string GetExceptionFrameName(Exception ex, int framesBack = 3)
        {
            try
            {
                var stacktrace = new StackTrace(ex);
                var f1 = stacktrace.GetFrame(framesBack);
                var m1 = f1.GetMethod();
                return m1.DeclaringType != null ? String.Format("{0}.{1}", (m1.DeclaringType).FullName, m1.Name) : m1.Name;
            }
            catch (Exception)
            {
                // nothing to do here
                return "Unknown";
            }
        }

        public static string GetFrameName(int framesBack = 3)
        {
            try
            {
                var stacktrace = new StackTrace();
                var f1 = stacktrace.GetFrame(framesBack);
                var m1 = f1.GetMethod();
                return m1.DeclaringType != null ? String.Format("{0}.{1}", (m1.DeclaringType).FullName, m1.Name) : m1.Name;
            }
            catch (Exception)
            {
                // nothing to do here
                return "Unknown";
            }
        }


    }
}
