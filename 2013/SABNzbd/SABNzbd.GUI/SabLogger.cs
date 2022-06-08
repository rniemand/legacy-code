using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RichardTestJson
{
    public sealed class SabLogger
    {
        // Singleton Constructor
        private static readonly SabLogger instance = new SabLogger();
        public static SabLogger Instance
        {
            get { return instance; }
        }

        // Private Stuffs
        public List<SabLoggerMessage> Messages { get; private set; }
        private int _maxMessages = 50;

        public delegate void NewMessageDelegate();
        public event NewMessageDelegate NewMessage;


        // Class Constructor
        private SabLogger()
        {
            Messages = new List<SabLoggerMessage>();
        }

        // Public Methods
        public void LogException(Exception ex)
        {
            try
            {
                PushMessage("Exception", ex.Message);
            }
            catch (Exception)
            {
                // OK then
            }
        }

        public void LogDebug(string message, params object[] o)
        {
            try
            {
                PushMessage("Debug", message, o);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public void LogWarning(string message, params object[] o)
        {
            try
            {
                PushMessage("Warning", message, o);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public void LogError(string message, params object[] o)
        {
            try
            {
                PushMessage("Error", message, o);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }



        private void PushMessage(string severity, string message, params object[] o)
        {
            try
            {
                Messages.Add(new SabLoggerMessage
                    {
                        TimeLogged = DateTime.Now,
                        Severity = severity,
                        Message = o.Length > 0 ? String.Format(message, o) : message,
                        CallingMethod = GetCallingMethod()
                    });

                if (NewMessage != null)
                    NewMessage();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        private static string GetCallingMethod()
        {
            try
            {
                var stackTrace = new StackTrace();
                var frame = stackTrace.GetFrame(3);
                var method = frame.GetMethod();

                return method.DeclaringType == null
                           ? method.Name
                           : String.Format("{0}.{1}", (method.DeclaringType).FullName, method.Name);
            }
            catch (Exception ex)
            {
                // Holder
            }

            return "n/a";
        }

    }
}
