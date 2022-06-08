using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Logging;

namespace Rn.Core.Helpers
{
    public static class XmlHelper
    {
        // =======================================>
        // Has Attribute
        public static bool HasAttribute(this XmlNode n, string attributeName)
        {
            if (n == null || n.Attributes == null || n.Attributes.Count == 0)
                return false;

            return n.Attributes.Cast<XmlAttribute>().Any(a => a.Name == attributeName);
        }


        // =======================================>
        // Get Attributes
        public static string GetAttribute(this XmlNode n, string name, string defaultValue = "")
        {
            if (n == null || !n.HasAttribute(name))
                return defaultValue;

            try
            {
                return n.Attributes[name].Value;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defaultValue;
            }
        }

        public static int GetAttributeInt(this XmlNode n, string name, int defaultValue = 0)
        {
            if (n == null || !n.HasAttribute(name))
                return defaultValue;

            try
            {
                return n.Attributes[name].Value.AsInt(defaultValue);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defaultValue;
            }
        }

        public static bool GetAttributeBool(this XmlNode n, string name, bool defaultValue = false)
        {
            if (n == null || !n.HasAttribute(name))
                return defaultValue;

            try
            {
                return n.Attributes[name].Value.AsBool(defaultValue);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defaultValue;
            }
        }

        public static long GetAttributeLong(this XmlNode n, string name, long defaultValue = 0)
        {
            if (n == null || !n.HasAttribute(name))
                return defaultValue;

            try
            {
                return n.Attributes[name].Value.AsLong(defaultValue);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defaultValue;
            }
        }


        // =======================================>
        // Get Key Values
        public static string GetKeyValue(this XmlNode n, string keyName, string defaultValue = "")
        {
            try
            {
                var node = n.SelectSingleNode(String.Format("Key[@Name='{0}']", keyName));

                if (node == null || !node.HasAttribute("Value"))
                    return defaultValue;

                return node.GetAttribute("Value", defaultValue);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defaultValue;
            }
        }

        public static bool GetKeyValueBool(this XmlNode n, string keyName, bool defaultValue = false)
        {
            try
            {
                var node = n.SelectSingleNode(String.Format("Key[@Name='{0}']", keyName));

                if (node == null || !node.HasAttribute("Value"))
                    return defaultValue;

                return node.GetAttributeBool("Value", defaultValue);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defaultValue;
            }
        }

        
        // =======================================>
        // Get Single Nodes Value
        public static bool HasNodeValue(this XmlNode n, string xpath)
        {
            if (n == null) return false;
            var node = n.SelectSingleNode(xpath);
            return node != null;
        }

        public static string GetNodeValue(this XmlNode n, string xpath, string defValue = "")
        {
            try
            {
                if (n == null) return defValue;
                var node = n.SelectSingleNode(xpath);
                return node == null ? defValue : node.InnerText;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return defValue;
            }
        }

        public static int GetNodeValueInt(this XmlNode n, string xpath, int defValue = 0)
        {
            var value = n.GetNodeValue(xpath, defValue.ToString());
            return String.IsNullOrEmpty(value) ? defValue : value.AsInt(defValue);
        }

        public static DateTime GetNodeValueDateTime(this XmlNode n, string xpath)
        {
            return n.GetNodeValue(xpath).AsDateTime();
        }

        public static DateTime GetNodeValueDateTime(this XmlNode n, string xpath, string format)
        {
            return n.GetNodeValue(xpath).AsDateTime(format);
        }

        public static bool GetNodeValueBool(this XmlNode n, string xpath, bool defValue = false)
        {
            return n.GetNodeValue(xpath, defValue.ToString()).AsBool(defValue);
        }

        public static double GetNodeValueDouble(this XmlNode n, string xpath, double defValue = 0)
        {
            var val = n.GetNodeValue(xpath, defValue.ToString());
            return String.IsNullOrEmpty(val) ? defValue : val.AsDouble(defValue);
        }

    }
}
