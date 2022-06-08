using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MySql.Data.MySqlClient;
using Rn.Configuration;
using Rn.Db;
using Rn.Helpers;
using Rn.Logging;

namespace Rn.Database
{
    public static class MySqlFactory
    {
        public static string ConfigObjName { get; internal set; }
        private static Dictionary<string, MySqlConnection> _connections;

        // =====> Class Constructor
        static MySqlFactory()
        {
            ConfigObjName = "default";
            _connections = new Dictionary<string, MySqlConnection>();
        }


        public static void SetConfigXmlName(string configName)
        {
            ConfigObjName = configName;
            Logger.LogInfo(String.Format("Setting Config Object Name to '{0}'", configName));
        }

        public static string CreateConnectionString(XmlNode node)
        {
            // check to see that this is a valid connection node
            if (node == null || node.Attributes == null || node.Attributes.Count == 0)
                return "";

            // Extract the values needed to create the SQL connection
            var cServer = node.GetAttribute("Server", "");
            var cDatabase = node.GetAttribute("Database", "");
            var cUser = node.GetAttribute("User", "");
            var cPass = node.GetAttribute("Password", "");

            // Return the generated connection string
            return CreateConnectionString(cServer, cDatabase, cUser, cPass);
        }

        public static string CreateConnectionString(string server, string database, string user = "", string pass = "")
        {
            var sb = new StringBuilder();
            sb.Append(String.Format("server = {0};", server));
            sb.Append(String.Format("database = {0};", database));

            if (!String.IsNullOrEmpty(user))
            {
                sb.Append(String.Format("user id = {0};", user));
                sb.Append(String.Format("password = {0};", pass));
            }
            else
            {
                // todo - Add code for a trusted connection
                Logger.LogDebug("Add code for a trusted connection!");
            }

            return sb.ToString();
        }

        public static bool HasConnection(string conName = "main")
        {
            return _connections.ContainsKey(conName);
        }

        public static void CreateConnection(string conName = "main")
        {
            // check to see if there is already a connection
            if (HasConnection(conName))
                return;

            if (!ConfigObj.GetConfig(ConfigObjName).HasDbNode(conName, DbConType.MySQL))
            {
                // there is no connection information in the config file
                Logger.LogWarning(String.Format(
                    "The requested MySQL Database Connection information for '{0}' was not found in the config file.",
                    conName));
                return;
            }

            try
            {
                // attempt to create the MySQL connection
                var node = ConfigObj.GetConfig(ConfigObjName).GetDbNode(conName, DbConType.MySQL);
                var openOnCreate = node.GetAttribute("OpenOnCreate", false);
                _connections.Add(conName, new MySqlConnection(CreateConnectionString(node)));
                
                // open the connection if we are asked to
                if (openOnCreate) _connections[conName].Open();
                Logger.LogInfo(String.Format(
                    "Successfully created MySQL Database Connection '{0}'",
                    conName));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "There was an error creating the MySQL Database Connection '{0}': {1}",
                    conName,
                    ex.Message));
            }
        }

        public static MySqlConnection GetConnection(string conName = "main")
        {
            // create the connection if it is not valid
            if (!HasConnection(conName))
                CreateConnection(conName);

            // still no connection, NULL it is
            return !HasConnection(conName) ? null : _connections[conName];
        }

        public static MySqlDataReader GetDataReader(string cmd, string conName = "main")
        {
            try
            {
                return new MySqlCommand(cmd, GetConnection(conName)).ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format("Error fetching MySqlDataReader: {0}", ex.Message));
                return null;
            }
        }

        public static void ExecuteNonQuery(string sql, string conName = "main")
        {
            try
            {
                var cmd = new MySqlCommand(sql, GetConnection(conName));
                cmd.ExecuteNonQuery();
                Logger.LogDebug(String.Format("Successfully ran MySQL query: {0}", sql));
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error running the MySQL query '{0}': {1}",
                    sql,
                    ex.Message));
            }
        }

    }
}
