using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Rn.Core.Configuration;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.Core.MySQL
{
    public static class MySQLFactory
    {

        public static MySqlConnection GetNewConnection(string conName = "default", bool openConnection = true)
        {
            try
            {
                // Get the connection XmlNode from config
                var xpath = String.Format("/{0}/DBConnections/Connection[@Type='MySQL' and @Name='{1}']", "{0}", conName);
                var conNode = Config.GetXmlConfig().SelectSingleNode(xpath, true);
                if (conNode == null) return null;

                // Create the connection object / open if needed
                var conInfo = new MySqlConnectionString(conNode);
                var dbCon = new MySqlConnection(conInfo.GetConnectionString());
                if (conInfo.AutoOpen || openConnection)
                    OpenConnection(ref dbCon);

                return dbCon;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return null;
            }
        }

        public static void OpenConnection(ref MySqlConnection con)
        {
            try
            {
                con.Open();
                Locale.LogEvent("rn.core", "0013", con.Database);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        public static bool ConnectionDefined(string conName = "default")
        {
            var xpath = String.Format("/{0}/DBConnections/Connection[@Type='MySQL' and @Name='{1}']", "{0}", "default");
            var conNode = Config.GetXmlConfig().SelectSingleNode(xpath, true);
            return conNode != null;
        }

        public static bool ConnectionEnabled(string conName = "default")
        {
            var xpath = String.Format("/{0}/DBConnections/Connection[@Type='MySQL' and @Name='{1}']", "{0}", "default");
            var conNode = Config.GetXmlConfig().SelectSingleNode(xpath, true);
            return conNode != null && conNode.GetAttributeBool("Enabled");
        }

    }
}
