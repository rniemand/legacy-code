using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Xml;
using Rn.Core.Configuration;
using Rn.Core.Helpers;

namespace Rn.Core.Logging
{
    public static class Locale
    {
        public static string LocaleShort { get; private set; }
        public static string LocaleDirectory { get; private set; }
        private static Dictionary<string, XmlDocument> _locales;

        static Locale()
        {
            try
            {
                _locales = new Dictionary<string, XmlDocument>();
                var localeNode = Config.GetXmlConfig().SelectSingleNode("/{0}/Locale", true);

                // Set the default locale information
                LocaleShort = localeNode.GetAttribute("Short", "en");
                var fullLocaleDir = localeNode.GetAttribute("Directory", "./XmlStrings");
                if (fullLocaleDir.Substring(fullLocaleDir.Length - 1, 1) != @"\")
                    fullLocaleDir = fullLocaleDir + @"\";
                fullLocaleDir = fullLocaleDir + LocaleShort;
                LocaleDirectory = IOHelper.FormatRelative(fullLocaleDir);
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "An exception was thrown loading the locale information: {0}", ex.Message), 100);
            }
        }


        private static void LoadLocaleFile(string localeFile)
        {
            // Check before we load the file
            if (_locales.ContainsKey(localeFile.ToLower().Trim()))
                return;

            // Check that the file exists
            var filePath = String.Format("{0}{1}.xml", LocaleDirectory, localeFile);
            if(!File.Exists(filePath))
            {
                Logger.LogWarning(String.Format(
                    "The requested locale file '{0}' could not be found!", filePath), 101);
                return;
            }

            try
            {
                // Attempt to load the file
                _locales.Add(localeFile.ToLower().Trim(), new XmlDocument());
                _locales[localeFile.ToLower().Trim()].Load(filePath);
                Logger.LogDebug(String.Format("Successfully loaded the locale file '{0}'", filePath), 103);
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "An exception was thrown trying to load the locale file '{0}': {1}", filePath, ex.Message), 102);
                return;
            }
        }

        private static LocaleStringResource GetLocaleString(string localeFile, string resourceName)
        {
            if(!LocaleFileLoaded(localeFile))
                return null;

            var keyName = localeFile.ToLower().Trim();
            var xpath = String.Format("/Rn.Core.Locale/StringResources/String[@Name='{0}']", resourceName);
            var nResource = _locales[keyName].SelectSingleNode(xpath);

            if (nResource == null || nResource.Attributes == null)
            {
                Logger.LogWarning(String.Format(
                    "The requested string resource '{0}' was not found in the locale file '{1}'", resourceName,
                    localeFile), 104);
                return null;
            }

            return new LocaleStringResource(nResource, localeFile);
        }


        public static void LogEvent(string localeFile, string resourceName, params object[] replace)
        {
            LoadLocaleFile(localeFile);
            
            var sr = GetLocaleString(localeFile, resourceName);
            if (sr == null || !sr.Enabled) return;
            
            if (replace.Length > 0)
                sr.FormatString(replace);

            Logger.LogEvent(sr);
        }

        public static bool LocaleFileLoaded(string localeFile)
        {
            return _locales.ContainsKey(localeFile.ToLower().Trim());
        }

    }
}
