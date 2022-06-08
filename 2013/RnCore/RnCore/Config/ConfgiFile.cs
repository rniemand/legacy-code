using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RnCore.Helpers;
using RnCore.Logging;

namespace RnCore.Config
{
    public class ConfgiFile
    {
        public string ConfigFilePath { get; private set; }
        public string ConfigFileName { get; private set; }
        public string RootElementName { get; private set; }
        public bool ConfigLoaded { get; private set; }

        private XmlDocument _configXml = null;

        public ConfgiFile(string filePath, string rootElName, string configName)
        {
            try
            {
                _configXml = new XmlDocument();
                _configXml.Load(filePath);

                ConfigFilePath = filePath;
                ConfigFileName = configName;
                RootElementName = rootElName;
                ConfigLoaded = true;
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
                ConfigLoaded = false;
            }
        }



        // Loggers
        public void RegisterLoggers()
        {
            try
            {
                var loggerNode = GetRootNode("Loggers");
                if (loggerNode == null || loggerNode.ChildNodes.Count==0)
                {
                    // todo: add to logger
                    Console.WriteLine("There are no logger nodes");
                    return;
                }

                foreach (XmlNode n in from XmlNode n in loggerNode where n.GetBooleanAttribute("Enabled") select n)
                    RnLogger.Loggers.CreateLogger(n);
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
            }
        }


        // XML Nodes
        public XmlNode GetRootNode(string nodeName)
        {
            if (!ConfigLoaded)
                return null;

            try
            {
                // todo: add to logger
                Console.WriteLine("Looking for XML node '{0}'", nodeName);

                var xpath = String.Format("/{0}/{1}", RootElementName, nodeName);
                return _configXml.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}
