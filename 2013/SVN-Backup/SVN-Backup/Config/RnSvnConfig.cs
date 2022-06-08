using System;
using SVN_Backup.Helpers;

namespace SVN_Backup.Config
{
    class RnSvnConfig
    {
        public string SvnUri { get; set; }
        public string DirCheckout { get; set; }
        public string DirExport { get; set; }
        public string ZipFileName { get; set; }
        public bool ZipEnabled { get; set; }


        public RnSvnConfig()
        {
            // holder...
        }

        public RnSvnConfig(string configName)
        {
            try
            {
                SvnUri = SvnGlobalConfig.Instance.GetRnSvnConfigKey(configName, "SvnURI");
                DirCheckout = SvnGlobalConfig.Instance.GetRnSvnConfigKey(configName, "DirCheckout");
                DirExport = SvnGlobalConfig.Instance.GetRnSvnConfigKey(configName, "DirExport");
                ZipFileName = SvnGlobalConfig.Instance.GetRnSvnConfigKey(configName, "ZipFileName");
                ZipEnabled = SvnGlobalConfig.Instance.GetRnSvnConfigKey(configName, "ZipEnabled").AsBool();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }

}