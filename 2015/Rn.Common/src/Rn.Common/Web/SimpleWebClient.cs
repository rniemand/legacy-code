using System.Net;
using Rn.Common.Interfaces;

namespace Rn.Common.Web
{
    public class SimpleWebClient : IWebClient
    {
        public SimpleWebClient()
        {
            
        }

        public string DownloadAsString(string url)
        {
            var wc = new WebClient();
            return wc.DownloadString(url);
        }
    }
}
