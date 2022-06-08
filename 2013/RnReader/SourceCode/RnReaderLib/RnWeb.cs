using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RnReaderLib
{
    public static class RnWeb
    {

        public static bool DownloadFile(string url, string filePath, bool replaceFile = false)
        {
            try
            {
                if (!replaceFile && File.Exists(filePath))
                {
                    RnLogger.LogWarn("Did not download '{0}' => '{0}' (already exists)", url, filePath);
                    return false;
                }

                RnIO.DeleteFile(filePath);

                using (var wc = new WebClient())
                {
                    wc.DownloadFile(url, filePath);
                }

                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static string GetUrlAsString(string url)
        {
            try
            {
                using (var wc = new WebClient())
                    return wc.DownloadString(url);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return String.Empty;
        }

    }
}
