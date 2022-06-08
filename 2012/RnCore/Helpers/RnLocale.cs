using System;
using System.Collections.Generic;
using System.IO;

namespace RnCore.Helpers
{
    // todo - LATER - hard code errors here

    public static class RnLocale
    {
        public static string Language { get; private set; }
        public static string Directory { get; private set; }
        public static string BaseDir { get; private set; }

        private static readonly Dictionary<string, RnObjLocale> Locales;


        static RnLocale()
        {
            Locales = new Dictionary<string, RnObjLocale>();
        }


        public static void SetPaths(string strLanguage, string strDirectory)
        {
            try
            {
                Language = strLanguage;
                Directory = strDirectory;
                BaseDir = String.Format(@"{0}{1}\", strDirectory, strLanguage);

                // Ensure that the RnLocale directory exists
                if (System.IO.Directory.Exists(BaseDir)) return;
                LogEvent("rn.common", "common.006", BaseDir);
            }
            catch (Exception ex)
            {
                LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public static bool LocaleFileLoaded(string xmlFile)
        {
            return Locales.ContainsKey(xmlFile);
        }

        private static bool LoadLocaleFile(string xmlFile)
        {
            try
            {
                var filePath = String.Format(@"{0}{1}.xml", BaseDir, xmlFile);

                if (!File.Exists(filePath))
                    return false;

                Locales.Add(xmlFile, new RnObjLocale(filePath));
                return true;
            }
            catch (Exception ex)
            {
                LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }

        public static void LogEvent(string xmlFile, string strName, params object[] replace)
        {
            xmlFile = xmlFile.ToLower().Trim();

            if (!LocaleFileLoaded(xmlFile) && !LoadLocaleFile(xmlFile))
                return;

            Locales[xmlFile].LogEvent(strName, replace);
        }

        // todo - LATER - add un-load locale method

    }
}
