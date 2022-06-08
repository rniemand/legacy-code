using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrlsToGo.RnUtils
{
    public sealed class RnPageScripts
    {
        private static readonly RnPageScripts instance = new RnPageScripts();
        public static RnPageScripts Instance{get { return instance; }}

        private readonly Dictionary<string, List<string>> _scripts;

        private RnPageScripts()
        {
            _scripts = new Dictionary<string, List<string>>();
        }


        public void AddScript(string relativeUrl, string type = "css")
        {
            type = type.ToLower().Trim();
            if (HasScript(relativeUrl, type))
                return;

            if (!_scripts.ContainsKey(type))
                _scripts.Add(type, new List<string>());

            _scripts[type].Add(relativeUrl);
        }

        public bool HasScript(string relativeUrl, string type = "css")
        {
            type = type.ToLower().Trim();
            if (_scripts.Count == 0 || !_scripts.ContainsKey(type) || _scripts[type].Count == 0)
                return false;

            return _scripts[type].Contains(relativeUrl);
        }

        public string[] GetScripts(string type = "css")
        {
            type = type.ToLower().Trim();
            if (!_scripts.ContainsKey(type) || _scripts[type].Count == 0)
                return new string[0];

            return _scripts[type].ToArray();
        }

    }
}