using System;
using System.Linq;
using System.Xml;
using RnCore.Logging;

namespace RnCore.Helpers
{
    public static class RnXml
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

        public static string GetAttributeString(this XmlNode n, string attributeName, string defaultValue = "")
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

        public static bool GetAttributeBool(this XmlNode n, string attributeName, bool defaultValue = false)
        {
            try
            {
                return n.GetAttributeString(attributeName, defaultValue.ToString()).AsBoolean();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static int GetAttributeInt(this XmlNode n, string attributeName, int defaultValue = 0)
        {
            try
            {
                return n.GetAttributeString(attributeName, defaultValue.ToString()).AsInt();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static double GetAttributeDbl(this XmlNode n, string attributeName, double defaultValue = 0)
        {
            try
            {
                return n.GetAttributeString(attributeName, defaultValue.ToString()).AsDouble();
            }
            catch (Exception ex)
            {
                ex.LogException();
                return defaultValue;
            }
        }

        public static DateTime GetAttributeDateTime(this XmlNode n, string attributeName)
        {
            var dudDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            try
            {
                var toParse = n.GetAttributeString(attributeName);
                if (String.IsNullOrEmpty(toParse))
                    return dudDate;

                return DateTime.Parse(toParse);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return dudDate;
            }
        }


    }
}
