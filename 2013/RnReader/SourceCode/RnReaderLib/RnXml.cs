using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RnReaderLib
{
    public static class RnXml
    {

        public static string GetInnerText(this XmlNode n, string xpath, string dValue = "")
        {
            try
            {
                var selectedNode = n.SelectSingleNode(xpath);
                return selectedNode == null ? dValue : selectedNode.InnerText.Trim();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return dValue;
        }

    }
}
