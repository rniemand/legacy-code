using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UrlsToGo.RnUtils
{
    public static class RnHelpers
    {
        private static Random _r;

        static RnHelpers()
        {
            _r = new Random(DateTime.Now.Millisecond);
        }

        public static int AsInt(this object o, int defaultValue = 0)
        {
            try
            {
                return int.Parse(o.ToString().ToLower().Trim());
            }
            catch (Exception ex)
            {
                // todo: log
            }

            return defaultValue;
        }

        public static double EpochTime()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static int GetRandom(int maxValue = 999)
        {
            return _r.Next(1, maxValue);
        }

        public static string Base64Encode(this string toEncode)
        {
            try
            {
                return Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(toEncode));
            }
            catch (Exception ex)
            {
                // todo: log
            }

            return String.Empty;
        }

        public static string Base64Decode(this string encodedData)
        {
            try
            {
                return System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(encodedData));
            }
            catch (Exception ex)
            {
                // todo: log this
            }

            return String.Empty;
        }

        public static string GetKeyValue(this FormCollection col, string keyName, string defaultVal = "")
        {
            try
            {
                // Check to see if we can find the requested key
                for (var i = 0; i < col.Count; i++)
                    if (col.Keys[i] == keyName)
                        return col.GetValue(keyName).AttemptedValue;

                // If the key was not found, return the default
                return defaultVal;
            }
            catch (Exception ex)
            {
                // todo: log this
            }

            return defaultVal;
        }

        public static string GetBaseUrl(string append = "")
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            if (String.IsNullOrEmpty(append))
                return baseUrl;
            return String.Format("{0}{1}", baseUrl, append);
        }

    }
}