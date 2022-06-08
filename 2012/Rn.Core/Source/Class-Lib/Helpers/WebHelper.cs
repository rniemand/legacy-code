using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Rn.Core.Logging;

namespace Rn.Core.Helpers
{
    public static class WebHelper
    {

        public static string GetUrlContents(string url)
        {
            try
            {
                Locale.LogEvent("rn.core", "0012", url);
                using (var client = new WebClient())
                    return client.DownloadString(url);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return "";
            }
        }

        public static XmlDocument GetUrlAsXml(string url)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(GetUrlContents(url));
                return doc;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return null;
            }
        }

        public static bool DownloadFile(string url, string saveTo, bool overwrite = false)
        {
            if (File.Exists(saveTo) && !overwrite)
            {
                Locale.LogEvent("rn.core", "0014", url, saveTo, "File already exists!");
                return false;
            }

            try
            {
                if (File.Exists(saveTo)) IOHelper.DeleteFile(saveTo);

                Locale.LogEvent("rn.core", "0015", url, saveTo);
                using (var client = new WebClient())
                    client.DownloadFile(url, saveTo);

                return true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }

    }
}
