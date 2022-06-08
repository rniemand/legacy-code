using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Logging;

namespace Rn.Helpers
{
    public static class XmlHelper
    {
        // Checking for attributes
        public static bool HasAttribute(this XmlNode node, string attName)
        {
            if (node == null) return false;
            if (node.Attributes != null && node.Attributes.Count == 0) return false;
            return node.Attributes.Cast<XmlAttribute>().Any(att => att.Name == attName);
        }

        // Fetching attributes
        public static string GetAttribute(this XmlNode node, string attName, string defaultValue)
        {
            if (!node.HasAttribute(attName))
                return defaultValue;

            return node.Attributes[attName].Value;
        }

        public static bool GetAttribute(this XmlNode node, string attName, bool defaultValue)
        {
            if (!node.HasAttribute(attName))
                return defaultValue;

            return node.Attributes[attName].Value.ToBool();
        }

        public static int GetAttribute(this XmlNode node, string attName, int defaultValue)
        {
            if (!node.HasAttribute(attName))
                return defaultValue;

            try
            {
                return int.Parse(node.Attributes[attName].Value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        // Checking that a node exists
        public static bool SubNodeExists(this XmlNode n, string nodeName)
        {
            var node = n.SelectSingleNode(nodeName);
            return node != null;
        }

        // Fetching nodes
        public static XmlNode GetSubNode(this XmlNode n, string nodeName)
        {
            if (!n.SubNodeExists(nodeName))
                return null;

            return n.SelectSingleNode(nodeName);
        }



        // Checking that a given Key node exists
        public static bool KeyNodeExists(this XmlNode n, string keyName)
        {
            var xpath = String.Format("Key[@Name='{0}']", keyName);
            var node = n.SelectSingleNode(xpath);
            return node != null;
        }

        // Fetching a key node
        public static XmlNode FetchKeyNode(this XmlNode n, string keyName)
        {
            if (!n.KeyNodeExists(keyName))
                return null;

            return n.SelectSingleNode(String.Format("Key[@Name='{0}']", keyName));
        }


        // Fetching Key Values
        public static string GetKeyAttribute(this XmlNode n, string keyName, string defaultValue = "")
        {
            var node = n.FetchKeyNode(keyName);

            if (node == null || !node.HasAttribute("Value"))
                return defaultValue;

            return node.Attributes["Value"].Value;
        }

        public static int GetKetAttributeInt(this XmlNode n, string keyName, int defaultValue = 0)
        {
            var node = n.FetchKeyNode(keyName);

            if (node == null || !node.HasAttribute("Value"))
                return defaultValue;

            try
            {
                return int.Parse(node.Attributes["Value"].Value);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error converting '{0}' to an int: {1}",
                    node.Attributes["Value"].Value,
                    ex.Message));

                return defaultValue;
            }
        }

        public static bool GetKetAttributeBool(this XmlNode n, string keyName, bool defaultValue = false)
        {
            var node = n.FetchKeyNode(keyName);

            if (node == null || !node.HasAttribute("Value"))
                return defaultValue;

            try
            {
                return node.Attributes["Value"].Value.ToBool();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error converting '{0}' to an int: {1}",
                    node.Attributes["Value"].Value,
                    ex.Message));

                return defaultValue;
            }
        }

    }
}
