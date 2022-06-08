using System;
using System.Xml;
using RnCore.Helpers;
using RnCore.Logging;

namespace RnCore
{
    class RnObjLocale
    {
        private XmlDocument _xml;
        public bool Ready { get; internal set; }
        public string XmlFilePath { get; private set; }


        public RnObjLocale(string filePath)
        {
            try
            {
                XmlFilePath = filePath;
                _xml = new XmlDocument();
                _xml.Load(XmlFilePath);
                Ready = true;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        public void LogEvent(string strName, params object[] replace)
        {
            try
            {
                // get the XML node
                var xpath = String.Format("/RnLocale/Strings/String[@Name='{0}' and @Enabled='true']", strName);
                var node = _xml.SelectSingleNode(xpath);

                if (node == null)
                {
                    // Cant find the Locale Key, this needs to be logged
                    var errMsg = String.Format(
                        "Cannot find Key '{0}' in Locale File '{1}', please check your spelling",
                        strName, XmlFilePath);
                    RnLogger.LogWarning(errMsg, 7);
                    return;
                }

                if (!node.GetAttrBool("Enabled")) return;

                // Format the messgae
                var msg = node.GetAttr("Message");
                if (replace.Length > 0)
                    msg = String.Format(msg, replace);

                // Decide on how to log the event
                var sev = RnLogger.AsLoggerSeverity(node.GetAttr("Severity", "warning"));
                switch (sev)
                {
                    case LoggerSeverity.Debug:
                        RnLogger.LogDebug(msg, node.GetAttrInt("EventId"));
                        return;

                    case LoggerSeverity.Warning:
                        RnLogger.LogWarning(msg, node.GetAttrInt("EventId"));
                        return;

                    case LoggerSeverity.Error:
                        RnLogger.LogError(msg, node.GetAttrInt("EventId"));
                        return;

                    case LoggerSeverity.Informational:
                        RnLogger.LogInfo(msg, node.GetAttrInt("EventId"));
                        return;

                    default:
                        RnLogger.LogDebug(msg, node.GetAttrInt("EventId"));
                        return;
                }
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


    }
}
