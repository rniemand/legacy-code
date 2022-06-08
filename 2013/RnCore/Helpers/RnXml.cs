using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace RnCore.Helpers
{
    public static class RnXml
    {

        // XML Attributes
        public static bool HasAttr(this XmlNode n, string attrName)
        {
            try
            {
                if (n == null || n.Attributes == null || n.Attributes.Count == 0)
                    return false;

                return n.Attributes.Cast<XmlAttribute>().Any(a => a.Name == attrName);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }

        public static string GetAttr(this XmlNode n, string attrName, string defValue = "")
        {
            if (!n.HasAttr(attrName))
                return defValue;

            try
            {
                return n.Attributes[attrName].Value;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

        public static bool GetAttrBool(this XmlNode n, string attrName, bool defValue = false)
        {
            if (!n.HasAttr(attrName))
                return defValue;

            try
            {
                return n.Attributes[attrName].Value.AsBool();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

        public static int GetAttrInt(this XmlNode n, string attrName, int defValue = 0)
        {
            if (!n.HasAttr(attrName))
                return defValue;

            try
            {
                return n.Attributes[attrName].Value.AsInt(defValue);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }

        public static double GetAttrDbl(this XmlNode n, string attrName, double defValue = 0)
        {
            if (!n.HasAttr(attrName))
                return defValue;

            try
            {
                return n.Attributes[attrName].Value.AsDbl(defValue);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }
        }


        // XML Root Elements
        public static bool HasChildNode(this XmlNode n, string nodeName)
        {
            try
            {
                var node = n.SelectSingleNode(nodeName);
                return node != null;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return false;
            }
        }

        public static XmlNode GetChildNode(this XmlNode n, string nodeName)
        {
            return n.SelectSingleNode(nodeName);
        }


        public static string GetChildNodeString(this XmlNode n, string nodeName, string defValue = "")
        {
            var cn = n.GetChildNode(nodeName);
            if (cn == null) return defValue;

            try
            {
                return cn.InnerText;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }

        }

        public static double GetChildNodeDbl(this XmlNode n, string nodeName, double defValue = 0)
        {
            var cn = n.GetChildNode(nodeName);
            if (cn == null) return defValue;

            try
            {
                return cn.InnerText.AsDbl(defValue);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }

        }

        public static int GetChildNodeInt(this XmlNode n, string nodeName, int defValue = 0)
        {
            var cn = n.GetChildNode(nodeName);
            if (cn == null) return defValue;

            try
            {
                return cn.InnerText.AsInt(defValue);
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
                return defValue;
            }

        }


    }
}
