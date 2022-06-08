using System.IO;
using System.Net;
using System.Text;
using NzbScraper.Common.Helpers;

namespace NzbScraper.DAL
{
    public class SimpleWebClient
    {
        public static string DownloadPageUsingGzip(string url)
        {
            string uncompressedData;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
                byte[] data = client.DownloadData(url);
                byte[] decompress = CompressionHelper.Decompress(data);
                uncompressedData = Encoding.ASCII.GetString(decompress);
            }

            return uncompressedData;
        }

        public static bool DownloadFile(string url, string destFile, bool replaceFile = false)
        {
            if (File.Exists(destFile) && !replaceFile)
            {
                return false;
            }

            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, destFile);
                return File.Exists(destFile);
            }
        }
    }
}
