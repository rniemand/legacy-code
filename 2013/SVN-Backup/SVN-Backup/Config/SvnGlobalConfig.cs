using System;
using System.Xml;
using SVN_Backup.Helpers;

namespace SVN_Backup.Config
{
    // http://msdn.microsoft.com/en-us/library/ff650316.aspx

    public sealed class SvnGlobalConfig
    {
        private static readonly SvnGlobalConfig inst = new SvnGlobalConfig();

        public static SvnGlobalConfig Instance
        {
            get { return inst; }
        }

        private SvnGlobalConfig()
        {
            ConfigLoaded = false;
            LoadConfigFile();
        }

        // Properties
        public bool ConfigLoaded { get; private set; }
        private XmlDocument _xml;

        // Helper Methods
        private void LoadConfigFile()
        {
            try
            {
                _xml = new XmlDocument();
                _xml.Load("./RnConfig.xml");
                ConfigLoaded = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public bool RnSvnConfigExists(string name)
        {
            if (!ConfigLoaded) return false;

            try
            {
                var xpath = String.Format("/RnConfig/RnSvnConfigs/RnSvnConfig[@Name='{0}']", name);
                var node = _xml.SelectSingleNode(xpath);
                return node != null;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public XmlNode GetRnSvnConfig(string name)
        {
            try
            {
                var xpath = String.Format("/RnConfig/RnSvnConfigs/RnSvnConfig[@Name='{0}']", name);
                return _xml.SelectSingleNode(xpath);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return null;
        }

        public string GetRnSvnConfigKey(string name, string keyName, string defValue = "")
        {
            try
            {
                var xpath = String.Format(
                    "/RnConfig/RnSvnConfigs/RnSvnConfig[@Name='{0}']/Key[@Name='{1}']",
                    name, keyName);
                var node = _xml.SelectSingleNode(xpath);

                if (node == null)
                {
                    // todo: log this
                    return defValue;
                }

                return node.InnerText.Trim();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return defValue;
        }

    }
}
