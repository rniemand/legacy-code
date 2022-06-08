using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using MySql.Data.MySqlClient;
using Rn.Configuration;
using Rn.Database;
using Rn.Logging;
using Rn.Helpers;

namespace Rn.DB.MySQL
{
    public class MySqlSp
    {
        public string SpName { get; internal set; }
        public string SpFilePath { get; internal set; }
        public bool SpFileFound { get; internal set; }
        public List<MySqlParameter> Parameters { get; internal set; }
        public string CommandText { get; internal set; }
        public MySqlConnection MySqlCon { get; internal set; }

        private XmlDocument _xml;
        private bool _xmlReady;
        
        // =====> Class constructors
        public MySqlSp(string spName, string conName = "main", string cfgName = "default")
        {
            // Map objects that we will be using...
            Parameters = new List<MySqlParameter>();
            MySqlCon = MySqlFactory.GetConnection(conName);
            SpName = spName;

            // check to see that the SP file exists
            var spFileDir = (ConfigObj.GetConfig(cfgName).GetConfigKeyValue("DB.MySQL.SP.Folder") + "/").Replace("//", "/");
            SpFilePath = String.Format("{0}{1}.xml", spFileDir, spName);
            SpFileFound = File.Exists(SpFilePath);
            
            if (!SpFileFound)
            {
                Logger.LogWarning(String.Format("Could not find the MySQL SP Command File '{0}'!", SpFilePath));
                return;
            }

            // load the SP file into memory
            LoadSpFile();
            LoadParameters();
            LoadCommandText();
        }

        private void LoadSpFile()
        {
            try
            {
                _xml = new XmlDocument();
                _xml.Load(SpFilePath);
                _xmlReady = true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error while trying to load SP file '{0}': {1}",
                    SpFilePath,
                    ex.Message));
            }
        }

        private void LoadParameters()
        {
            if (!_xmlReady) return;

            var nodes = _xml.SelectNodes("/MySQLSP/Parameters/Parameter");
            if (nodes == null || nodes.Count == 0) return;

            // load the parameters into memory
            foreach (XmlNode node in nodes)
                Parameters.Add(new MySqlParameter(node));
        }

        private void LoadCommandText()
        {
            try
            {
                var node = _xml.SelectSingleNode("/MySQLSP/CommandText");
                CommandText = node.InnerText;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format(
                    "Error while setting MySQL SP's Command Text: {0}",
                    ex.Message));
            }
        }

        
        // =====> Public methods
        public void AddParameter(string name, string value)
        {
            if (Parameters.Count == 0) return;




            foreach (var parameter in Parameters.Where(parameter => parameter.Name == name))
            {
                parameter.SetValue(value);
                return;
            }

            // todo - log there was no value to set
        }

        public string GenerateSql()
        {
            if (Parameters.Count == 0)
                return CommandText;

            // todo - create a RX helper for some of these commands
            var processedCmd = CommandText;
            foreach (var p in Parameters)
            {
                processedCmd = Regex.Replace(processedCmd, p.CommandTextName, p.Value,
                                             RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }

            return processedCmd;
        }

        public void ExecuteNonQuery()
        {
            try
            {
                using (var cmdObj = new MySqlCommand(GenerateSql(), MySqlCon))
                    cmdObj.ExecuteNonQuery();

                Logger.LogDebug(String.Format("Successfully executed the MySQL SP '{0}'", SpName));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "There was an error executing the MySQL SP '{0}': {1}",
                    SpName,
                    ex.Message));
            }

        }
    }
}
