using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace RnCore.Helpers
{
    public static class RnWeb
    {

        public static string GetUrl(string url)
        {
            var data = "";

            try
            {
                using (var wc = new WebClient())
                {
                    data = wc.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }

            return data;
        }

        public static XmlDocument GetUrlAsXml(string url)
        {
            try
            {
                RnLocale.LogEvent("rn.common", "common.006", url);
                var doc = new XmlDocument();
                doc.Load(url);
                return doc;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }

    }
}
