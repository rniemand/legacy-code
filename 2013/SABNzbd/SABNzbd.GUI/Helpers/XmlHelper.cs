using System;
using System.Xml;

namespace RichardTestJson.Helpers
{
    static class XmlHelper
    {

        public static string GetNodeValString(this XmlNode n, string nodeName)
        {
            if (n == null)
                return String.Empty;

            try
            {
                var node = n.SelectSingleNode(nodeName);
                if (node == null) return String.Empty;
                return String.IsNullOrEmpty(node.InnerText) ? String.Empty : node.InnerText;
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
                return String.Empty;
            }
        }

        public static double GetNodeValDouble(this XmlNode n, string nodeName)
        {
            if (n == null)
                return 0;

            try
            {
                var node = n.SelectSingleNode(nodeName);
                if (node == null) return 0;
                return String.IsNullOrEmpty(node.InnerText) ? 0 : double.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
                return 0;
            }
        }



    }
}
