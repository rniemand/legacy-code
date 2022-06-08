using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace RichardTestJson
{
    class SabWarning
    {
        public string Date { get; private set; }
        public string Severity { get; private set; }
        public string Warning { get; private set; }

        public SabWarning(XmlNode n)
        {
            try
            {
                var w = n.InnerText;
                var parts = w.Split(Convert.ToChar("\n"));

                Date = parts[0];
                Severity = parts[1];
                Warning = parts[2];
            }
            catch (Exception ex)
            {
                SabLogger.Instance.LogException(ex);
            }
        }
    }
}
