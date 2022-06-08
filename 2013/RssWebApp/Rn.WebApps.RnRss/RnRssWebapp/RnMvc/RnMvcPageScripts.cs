using System.Collections.Generic;

namespace Rn.WebApps.RnRss.Web.RnMvc
{
    public sealed class RnMvcPageScripts
    {
        private static readonly RnMvcPageScripts instance = new RnMvcPageScripts();
        public static RnMvcPageScripts Instance
        {
            get { return instance; }
        }

        private RnMvcPageScripts()
        {
            _cssCache = new List<string>();
            _jsCache = new List<string>();
        }


        private readonly List<string> _cssCache;
        private readonly List<string> _jsCache;


        public void AddJsFile(string filePath)
        {
            if (!_jsCache.Contains(filePath))
                _jsCache.Add(filePath);
        }

        public void AddCssFile(string filePath)
        {
            if (!_cssCache.Contains(filePath))
                _cssCache.Add(filePath);
        }

        public string[] GetCssFiles()
        {
            return _cssCache.ToArray();
        }

        public string[] GetJsFiles()
        {
            return _jsCache.ToArray();
        }

    }
}