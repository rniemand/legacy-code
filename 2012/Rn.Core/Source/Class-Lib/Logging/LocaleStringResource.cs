using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;

namespace Rn.Core.Logging
{
    public class LocaleStringResource
    {
        public string Name { get; private set; }
        public int EventId { get; private set; }
        public LogSeverity Severity { get; private set; }
        public bool Enabled { get; private set; }
        public string Value { get; private set; }
        public string LocaleFile { get; private set; }

        public LocaleStringResource(XmlNode n, string localeFile)
        {
            try
            {
                Name = n.GetAttribute("Name");
                EventId = n.GetAttributeInt("EventID");
                Severity = n.GetAttribute("Severity", "Debug").AsLogSeverity();
                Enabled = n.GetAttributeBool("Enabled");
                Value = n.GetAttribute("Value");
                LocaleFile = localeFile;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Exception creating a new instance of LocaleStringResource: {0}", ex.Message), 105);
            }
        }

        public void FormatString(params object[] replace)
        {
            if (replace.Length == 0) return;

            try
            {
                Value = String.Format(Value, replace);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "An exception was thrown while trying to format the string resource '{0}' in locale file '{1}': {2}",
                    Name, LocaleFile, ex.Message), 106);
            }
        }

    }
}
