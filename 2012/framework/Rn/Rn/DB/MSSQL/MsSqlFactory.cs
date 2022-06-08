using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Configuration;
using Rn.Helpers;
using Rn.Logging;

namespace Rn.DB.MsSql
{
    public static class MsSqlFactory
    {
        public static string ConfigObjName { get; internal set; }
        private static Dictionary<string, SqlConnection> _connections;

        static MsSqlFactory()
        {
            _connections = new Dictionary<string, SqlConnection>();
        }

        public static void SetConfigName(string name = "default")
        {
            ConfigObjName = name;
        }


        public static SqlConnection CreateConnection(XmlNode n)
        {
            var cName = n.GetAttribute("Name", "");
            if (String.IsNullOrEmpty(cName))
            {
                Logger.LogWarning("Cannot create a MsSqlConnection with the name set to NULL!");
                return null;
            }

            try
            {
                // Check to see if there is already a connection for this name
                if (HasConnection(cName))
                    return _connections[cName];

                // Create the connection
                var cStr = CreateConnectionStr(n);
                var cOpen = n.GetAttribute("OpenOnCreate", false);
                _connections.Add(cName, new SqlConnection(cStr));
                if (cOpen) OpenConnection(_connections[cName]);
                return _connections[cName];
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "Error while trying to create the MsSqlConnection '{0}': {1}",
                    cName, ex.Message));
                return null;
            }
        }

        public static void OpenConnection(SqlConnection con)
        {
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format("Could not open the given SqlConnection: {0}", ex.Message));
            }
        }


        // =====> Connection String
        public static string CreateConnectionStr(XmlNode n)
        {
            // what we need to create the connection string
            var cServer = n.GetAttribute("Server", "");
            var cDb = n.GetAttribute("Database", "");
            var cUser = n.GetAttribute("User", "");
            var cPass = n.GetAttribute("Password", "");

            return CreateConnectionStr(cServer, cDb, cUser, cPass);
        }

        public static string CreateConnectionStr(string svr, string db, string user = "", string pass = "")
        {
            var sb = new StringBuilder();
            sb.Append(String.Format("Server={0};", svr));
            sb.Append(String.Format("Database={0};", db));

            if (String.IsNullOrEmpty(user))
                sb.Append("Trusted_Connection=True;");
            else
            {
                sb.Append(String.Format("User Id={0};", user));
                sb.Append(String.Format("Password={0};", user));
            }

            return sb.ToString();
        }



        public static bool HasConnection(string conName)
        {
            return _connections.Count != 0 && _connections.ContainsKey(conName);
        }

        public static bool ConnectionDefined(string conName = "default")
        {
            try
            {
                // check to see if the connection has been defined in the config file
                var rNode = ConfigObj.GetConfig(ConfigObjName).GetRootNode("DBConnections");
                const string xpath = "Connection[@Type='MSSQL' and @Enabled='true']";
                var node = rNode.SelectSingleNode(xpath);
                return node != null && node.Attributes != null && node.Attributes.Count != 0;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error while checking if the MSSQL DB Connection '{0}' is defined: {1}",
                    conName, ex.Message));
                return false;
            }
        }

        public static SqlConnection GetConnection(string conName = "default")
        {
            try
            {
                // look for the connection node
                var rNode = ConfigObj.GetConfig(ConfigObjName).GetRootNode("DBConnections");
                const string xpath = "Connection[@Type='MSSQL' and @Enabled='true']";
                var node = rNode.SelectSingleNode(xpath);
                
                if (node == null || node.Attributes == null)
                    return null;

                return CreateConnection(node);
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "Error creating the MsSqlConnection '{0}': {1}",
                    conName, ex.Message));
                return null;
            }
        }

        public static string QueryToString(SqlCommand cmd)
        {
            var sb = new StringBuilder();
            sb.Append(cmd.CommandText);

            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                sb.Append(Environment.NewLine);
                if (cmd.Parameters.Count > 0)
                {
                    var pCount = cmd.Parameters.Count - 1;
                    var pCounter = 0;
                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        if (p.SqlDbType == SqlDbType.BigInt || p.SqlDbType == SqlDbType.Int)
                            sb.Append(String.Format("\t@{0} = {1}", p.ParameterName, p.Value));
                        else
                            sb.Append(String.Format("\t@{0} = '{1}'", p.ParameterName, p.Value));

                        if (pCounter < pCount) sb.Append(",");
                        sb.Append(Environment.NewLine);
                        pCounter++;
                    }
                }
            }

            return sb.ToString();
        }

    }
}
