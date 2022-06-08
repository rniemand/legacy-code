using System;
using System.Linq;
using System.Xml;
using RnCore.Logging;

namespace RnCore.Helpers
{
    public static class XmlHelper
    {

        // Working with XmlNode Attributes
        public static bool HasAttribute(this XmlNode n, string attributeName)
        {
            try
            {
                if (n == null || n.Attributes == null || n.Attributes.Count == 0)
                    return false;
                return n.Attributes.Cast<XmlAttribute>().Any(a => a.Name == attributeName);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }

        public static string GetStringAttribute(this XmlNode n, string attributeName, string defaultValue = "")
        {
            try
            {
                return !n.HasAttribute(attributeName) ? defaultValue : n.Attributes[attributeName].Value;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static bool GetBooleanAttribute(this XmlNode n, string attributeName, bool defaultValue = false)
        {
            try
            {
                return n.GetStringAttribute(attributeName, defaultValue.ToString()).AsBoolean();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static int GetIntAttribut(this XmlNode n, string attributeName, int defaultValue = 0)
        {
            try
            {
                return n.GetStringAttribute(attributeName, defaultValue.ToString()).AsInt();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static double GetDoubleAttribute(this XmlNode n, string attributeName, double defaultValue = 0)
        {
            try
            {
                return n.GetStringAttribute(attributeName, defaultValue.ToString()).AsDouble();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }


    }
}
