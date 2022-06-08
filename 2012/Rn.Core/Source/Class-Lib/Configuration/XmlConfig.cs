using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.Core.Configuration
{
    public class XmlConfig
    {
        public string FilePath { get; private set; }
        public string RootNode { get; private set; }
        public string ConfigName { get; private set; }
        public bool XmlLoaded { get; private set; }

        private XmlDocument _xml;

        
        // Class Constructor
        public XmlConfig(string rootNode, string filePath, string name)
        {
            // Set default values
            FilePath = filePath.Replace("./", AppDomain.CurrentDomain.BaseDirectory);
            RootNode = rootNode;
            ConfigName = name;
            XmlLoaded = false;

            // Load the config file
            LoadConfigFile();
        }

        private void LoadConfigFile()
        {
            if (!File.Exists(FilePath))
            {
                Locale.LogEvent("rn.core.common", "0001", FilePath);
                return;
            }

            try
            {
                _xml = new XmlDocument();
                _xml.Load(FilePath);
                XmlLoaded = true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        public XmlNode GetRootNode()
        {
            return _xml.SelectSingleNode(String.Format("/{0}", RootNode));
        }

        public XmlNode SelectSingleNode(string xpath, bool formatString = false)
        {
            if (formatString)
                return _xml.SelectSingleNode((String.Format(xpath, RootNode)));

            return _xml.SelectSingleNode(xpath);
        }

        public XmlNodeList SelectNodes(string xpath, bool formatString = false)
        {
            if (formatString)
                return _xml.SelectNodes(String.Format(xpath, RootNode));

            return _xml.SelectNodes(xpath);
        }


        // Config Keys
        public string GetConfigKey(string name, string defaultValue = "")
        {
            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']", RootNode, name);
            var node = _xml.SelectSingleNode(xpath);
            return node == null ? defaultValue : node.GetAttribute("Value", defaultValue);
        }

        public int GetConfigKeyInt(string name, int defaultValue = 0)
        {
            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']", RootNode, name);
            var node = _xml.SelectSingleNode(xpath);
            return node == null ? defaultValue : node.GetAttributeInt("Value", defaultValue);
        }

        public bool GetConfigKeyBool(string name, bool defaultValue = false)
        {
            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']", RootNode, name);
            var node = _xml.SelectSingleNode(xpath);
            return node == null ? defaultValue : node.GetAttributeBool("Value", defaultValue);
        }

        public long GetConfigKeyLong(string name, long defaultValue = 0)
        {
            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']", RootNode, name);
            var node = _xml.SelectSingleNode(xpath);
            return node == null ? defaultValue : node.GetAttributeLong("Value", defaultValue);
        }

        // Loggers
        public void CreateLoggers()
        {
            try
            {
                var xpath = String.Format("/{0}/Loggers/Logger[@Enabled='true']", RootNode);
                var nodes = _xml.SelectNodes(xpath);

                if (nodes == null || nodes.Count == 0)
                    return;

                foreach (XmlNode n in nodes)
                    Logger.AddLogger(n);

                Locale.LogEvent("rn.core", "0004", nodes.Count);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core", "0005", ex.Message);
            }
        }


    }
}
