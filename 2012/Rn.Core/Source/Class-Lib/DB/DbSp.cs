using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Rn.Core.Configuration;
using Rn.Core.Logging;
using Rn.Core.Helpers;

namespace Rn.Core.DB
{
    public static class DbSp
    {
        public static string SpRootDir { get; private set; }

        static DbSp()
        {
            try
            {
                var node = Config.GetXmlConfig().SelectSingleNode("/{0}/DbSp", true);
                SpRootDir = node == null ? "./DbResources/{0}/SP/" : node.GetAttribute("Folder");
                SpRootDir = SpRootDir.Replace("./", "<<0>>").Replace("/", @"\").Replace("<<0>>", "./");
                SpRootDir = IOHelper.FormatRelative(SpRootDir);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        public static StoredProc GetSp(string spName, DbConType ct)
        {
            try
            {
                var spFile = String.Format("{0}{1}.xml", String.Format(SpRootDir, ct.AsString()), spName);

                if (!File.Exists(spFile))
                {
                    Locale.LogEvent("rn.core.common", "0001", spFile);
                    return null;
                }

                return new StoredProc(spFile);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return null;
            }
        }

    }
}
