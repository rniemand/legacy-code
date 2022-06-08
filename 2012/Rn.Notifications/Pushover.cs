using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Rn.Core.Logging;
using Rn.Core.Helpers;

// https://pushover.net/api
// https://pushover.net/faq

namespace Rn.Notifications
{
    public class Pushover
    {
        private readonly string _appToken;
        public string ApiUrl { get; private set; }


        // =====> Class constructor and methods
        public Pushover(string appToken)
        {
            _appToken = appToken;
            ApiUrl = "https://api.pushover.net/1/messages.json";
        }

        private bool PushMessage(NameValueCollection data)
        {
            bool success;

            try
            {
                data.Add(new NameValueCollection { { "token", _appToken } });

                using (var client = new WebClient())
                {
                    client.UploadValues(ApiUrl, data);
                    success = client.ResponseHeaders["Status"].RxIsMatch("200.*");
                }
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "rn.common.002", ex.Message);
                success = false;
            }

            return success;
        }

        private static string CastDate(DateTime d)
        {
            try
            {
                var dt = d.AsUnixTimeStamp().ToString(CultureInfo.InvariantCulture).Split('.');
                return dt[0];
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "rn.common.002", ex.Message);
                return "0";
            }
        }


        // =====> Basic Message
        public bool SendMessage(string userKey, string message, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"priority", (highPriority ? "1" : "0")}
                           };

            return PushMessage(data);
        }

        public bool SendMessage(string userKey, string message, DateTime time, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"priority", (highPriority ? "1" : "0")},
                               {"timestamp", CastDate(time)}
                           };

            return PushMessage(data);
        }


        // =====> Basic Message + Title
        public bool SendMessage(string userKey, string message, string title, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"title", title},
                               {"priority", (highPriority ? "1" : "0")}
                           };

            return PushMessage(data);
        }

        public bool SendMessage(string userKey, string message, string title, DateTime time, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"title", title},
                               {"priority", (highPriority ? "1" : "0")},
                               {"timestamp", CastDate(time)}
                           };

            return PushMessage(data);
        }


        // =====> URL Message
        public bool SendMessage(string userKey, string message, string title, string url, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"title", title},
                               {"url", url},
                               {"priority", (highPriority ? "1" : "0")}
                           };

            return PushMessage(data);
        }

        public bool SendMessage(string userKey, string message, string title, string url, DateTime time, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"title", title},
                               {"url", url},
                               {"priority", (highPriority ? "1" : "0")},
                               {"timestamp", CastDate(time)}
                           };

            return PushMessage(data);
        }


        // =====> URL Message + Title
        public bool SendMessage(string userKey, string message, string title, string url, string urlTitle, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"title", title},
                               {"url", url},
                               {"url_title", urlTitle},
                               {"priority", (highPriority ? "1" : "0")}
                           };

            return PushMessage(data);
        }

        public bool SendMessage(string userKey, string message, string title, string url, string urlTitle, DateTime time, bool highPriority = false)
        {
            var data = new NameValueCollection
                           {
                               {"user", userKey},
                               {"message", message},
                               {"title", title},
                               {"url", url},
                               {"url_title", urlTitle},
                               {"priority", (highPriority ? "1" : "0")},
                               {"timestamp", CastDate(time)}
                           };

            return PushMessage(data);
        }

    }
}
