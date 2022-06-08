using System;
using System.Xml;
using Rn.Db;
using Rn.Logging;
using Rn.Helpers;

namespace Rn.Configuration
{
    public class ConfigXml
    {
        public string FilePath { get; internal set; }
        public string RootNode { get; internal set; }
        public bool ConfigFileLoaded { get; internal set; }

        private XmlDocument _configXml;


        public void SetNodeAttributeValue(string xpath, string attributeName, string value)
        {
            try
            {
                // Ensure that we have something to work with
                var node = _configXml.SelectSingleNode(xpath);
                if (node == null)
                {
                    Logger.LogWarning(String.Format("The XML element '{0}' could not be found", xpath));
                    return;
                }

                // Set or create the requested attribute
                if (node.Attributes == null)
                {
                    // todo - we will need to add in the attribute
                }
                else
                {
                    node.Attributes[attributeName].Value = value;
                }

                _configXml.Save(FilePath);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "An exception was thrown trying to set the value for '{0}' to '{1}' for the XML element '{2}': {3}",
                    attributeName, value, xpath, ex.Message));
            }
            
        }


        // =================================================================>
        // =====> Constructor
        public ConfigXml(string filePath, string rootNode)
        {
            FilePath = filePath;
            RootNode = rootNode;

            LoadConfigFile();
        }

        private void LoadConfigFile()
        {
            try
            {
                _configXml = new XmlDocument();
                _configXml.Load(FilePath);
                ConfigFileLoaded = true;

                Logger.LogInfo(String.Format("Successfully loaded the config file '{0}'", FilePath));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error loading config file '{0}': {1}", FilePath, ex.Message));
            }
        }


        // =================================================================>
        // =====> Root Node
        public bool HasRootNode(string nodeName)
        {
            var xpath = String.Format("/{0}/{1}", RootNode, nodeName);
            var node = _configXml.SelectNodes(xpath);
            return node != null;
        }

        public XmlNode GetRootNode(string nodeName)
        {
            if (!HasRootNode(nodeName))
                return null;

            return _configXml.SelectSingleNode(String.Format("/{0}/{1}", RootNode, nodeName));
        }


        // =================================================================>
        // =====> ConfigKeys Keys
        public bool HasConfigKey(string keyName)
        {
            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']/@Value", RootNode, keyName);
            var node = _configXml.SelectSingleNode(xpath);
            return node != null;
        }

        public XmlNode GetConfigKeyNode(string keyName)
        {
            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']", RootNode, keyName);
            return _configXml.SelectSingleNode(xpath);
        }

        public string GetConfigKeyValue(string keyName, string defaultValue = "")
        {
            if (!HasConfigKey(keyName)) return defaultValue;

            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']/@Value", RootNode, keyName);
            var node = _configXml.SelectSingleNode(xpath);
            if (node == null) return defaultValue;

            return node.Value;
        }

        public int GetConfigKeyValue(string keyName, int defaultValue)
        {
            if (!HasConfigKey(keyName)) return defaultValue;

            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']/@Value", RootNode, keyName);
            var node = _configXml.SelectSingleNode(xpath);

            return node == null ? defaultValue : node.Value.ToInt(defaultValue);
        }

        public bool GetConfigKeyValue(string keyName, bool defaultValue)
        {
            if (!HasConfigKey(keyName)) return defaultValue;

            var xpath = String.Format("/{0}/ConfigKeys/Key[@Name='{1}']/@Value", RootNode, keyName);
            var node = _configXml.SelectSingleNode(xpath);

            return node == null ? defaultValue : node.Value.ToBool(defaultValue);
        }



        // =================================================================>
        // =====> Generic Configuration Calls
        public void CreateLoggers()
        {
            var xpath = String.Format("/{0}/Loggers/Logger[@Enabled='true']", RootNode);
            var nodes = _configXml.SelectNodes(xpath);

            if (nodes != null && nodes.Count == 0)
                return;

            foreach (XmlNode node in nodes)
                Logger.CreateLogger(node);
        }




        public bool HasDbNode(string conName, DbConType conType)
        {
            if (conType == DbConType.MySQL)
            {
                var xpath = String.Format(
                    "/{0}/DBConnections/Connection[@Type='MySQL' and @Name='{1}']",
                    RootNode,
                    conName);

                var node = _configXml.SelectSingleNode(xpath);

                return node != null;
            }

            return false;
        }

        public XmlNode GetDbNode(string conName, DbConType conType)
        {
            if (conType == DbConType.MySQL)
            {
                var xpath = String.Format(
                    "/{0}/DBConnections/Connection[@Type='MySQL' and @Name='{1}']",
                    RootNode,
                    conName);
                return _configXml.SelectSingleNode(xpath);
            }

            return null;
        }
    }
}
