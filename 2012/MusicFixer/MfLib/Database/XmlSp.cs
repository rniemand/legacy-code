using System;
using System.IO;
using System.Xml;
using RnCore.Helpers;

namespace MfLib.Database
{
    public class XmlSp
    {
        public string BaseDir { get; private set; }
        public string XmlSpFilePath { get; private set; }
        public bool XmlSpFileFound { get; private set; }
        public string Name { get; private set; }
        public string Version { get; private set; }

        private readonly XmlDocument _xml;

        public XmlSp(string baseDir, string fileName, string version = "1.0.0")
        {
            try
            {
                // Setup generics
                BaseDir = baseDir;
                XmlSpFilePath = String.Format("{0}{1}.[{2}].xml", BaseDir, fileName, version);
                XmlSpFileFound = File.Exists(XmlSpFilePath);
                Name = fileName;
                Version = version;

                // Load the xml file into memory
                if (!XmlSpFileFound)
                {
                    // todo - LATER - add logic to alert if the sp file could not be found - this was a pain...
                    RnLocale.LogEvent("rn.common", "common.002", XmlSpFilePath);
                    return;
                }
                _xml = new XmlDocument();
                _xml.Load(XmlSpFilePath);

                // Log that we were successful
                RnLocale.LogEvent("mf", "mf.0006", Name, Version);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


        public XmlNode GetSpNode(string name)
        {
            try
            {
                var xpath = String.Format("/XmlSp/Procedures/SP[@Name='{0}']", name);
                return _xml.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return null;
            }
        }

        public string GetSp(string name, params object[] parameters)
        {
            var n = GetSpNode(name);
            var cmdText = String.Empty;

            try
            {
                if (n != null)
                {
                    cmdText = n.InnerText;

                    if (parameters.Length > 0)
                    {
                        var pFixed = new object[parameters.Length];
                        var pPos = 0;
                        
                        foreach (var p in parameters)
                            pFixed[pPos++] = p.ToString().Replace("'", "''");

                        cmdText = String.Format(cmdText, pFixed);
                    }

                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
            
            return cmdText;
        }


    }
}
