using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Configuration;
using Rn.DB.MsSql;
using Rn.Logging;
using Rn.Helpers;

namespace AlertProcessor.Classes
{
    public static class GlobalObj
    {
        public static ConfigXml Config { get; private set; }
        public static List<ScomAlert> LoadedAlerts { get; private set; }
        public static List<RxPattern> LoadedPatterns { get; private set; }
        public static StringBuilder CurrentQry { get; private set; }

        public static List<string> AlertIgnoreExact { get; private set; }
        public static List<string> AlertIgnoreLike { get; private set; }
        public static List<string> AlertIncludeExact { get; private set; }

        static GlobalObj()
        {
            Config = ConfigObj.GetConfig();
            LoadedAlerts = new List<ScomAlert>();
            AlertIgnoreExact = new List<string>();
            AlertIncludeExact = new List<string>();
            AlertIgnoreLike = new List<string>();
            LoadedPatterns = new List<RxPattern>();
            CurrentQry = new StringBuilder();
        }


        // =====> Public Methods
        public static void LoadRxPatterns()
        {
            var filePath = ConfigObj.GetConfig().GetConfigKeyValue("App.Files.RxPatterns");
            if (!File.Exists(filePath))
            {
                Logger.LogError(String.Format("Could not find RxPatterns file '{0}'", filePath));
                return;
            }

            try
            {
                // check to see we have patterns
                Logger.LogDebug(String.Format("Attempting to load RxPatterns file '{0}'...", filePath));
                var xml = new XmlDocument();
                xml.Load(filePath);
                var nodes = xml.SelectNodes("/RxPatterns/Pattern[@Enabled='true']");
                if (nodes == null || nodes.Count == 0)
                {
                    Logger.LogError(String.Format("There were no RxPatterns defined in '{0}'.", filePath));
                    return;
                }

                // register the patterns with the parser
                foreach (XmlNode n in nodes)
                    LoadedPatterns.Add(new RxPattern(n));

                // log our success
                Logger.LogInfo(String.Format(
                    "Successfully loaded '{0}' RxPatterns from the RxPattern file '{1}'",
                    nodes.Count, filePath));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error loading RxPatterns file '{0}': {1}", filePath, ex.Message));
            }
        }

        public static void LoadAlertFilters()
        {
            try
            {
                var rNode = ConfigObj.GetConfig().GetRootNode("AlertFilters");

                // ================================================>
                // Ignore Exact Matches
                var ignEx = rNode.SelectSingleNode("IgnoreExact");
                if (ignEx != null)
                {
                    var ignExNodes = ignEx.SelectNodes("AlertName[@Enabled='true']");
                    if (ignExNodes != null && ignExNodes.Count > 0)
                    {
                        foreach (XmlNode n in ignExNodes)
                            AlertFilterIgnoreExact(n.GetAttribute("Name", ""));
                    }
                }

                // ================================================>
                // Ignore Loose Matches
                var ignLoo = rNode.SelectSingleNode("IgnoreLike");
                if (ignLoo != null)
                {
                    var ignLooNodes = ignLoo.SelectNodes("AlertName[@Enabled='true']");
                    if (ignLooNodes == null || ignLooNodes.Count <= 0) return;
                    foreach (XmlNode n in ignLooNodes)
                        AlertFilterIgnoreLike(n.GetAttribute("Name", ""));
                }

                // ================================================>
                // Include Exactly
                var incEx = rNode.SelectSingleNode("IncludeExact");
                if (incEx != null)
                {
                    var incExNodes = incEx.SelectNodes("AlertName[@Enabled='true']");
                    if (incExNodes == null || incExNodes.Count <= 0) return;
                    foreach (XmlNode n in incExNodes)
                        AlertFilterIncludeExact(n.GetAttribute("Name", ""));
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format("Error loading ignore list: {0}", ex.Message));
            }
        }

        public static void LoadAlerts()
        {
            LoadedAlerts.Clear();

            try
            {
                // Attempt to get alerts from the SQL database
                var cmd = new SqlCommand
                {
                    CommandText = CreateSqlCommand(),
                    Connection = MsSqlFactory.GetConnection(),
                    CommandType = CommandType.Text
                };

                // Extract the alerts from the DB
                var rowCount = 1;
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        Logger.LogDebug(String.Format("Loading alert: {0}", rowCount));
                        LoadedAlerts.Add(new ScomAlert
                                             {
                                                 OriginalDescription = reader["AlertDescription"].ToString().Trim(),
                                                 AlertName = reader["AlertName"].ToString().Trim(),
                                                 Severity = reader["Severity"].ToInt(),
                                                 Priority = reader["Priority"].ToInt(),
                                                 Category = reader["Category"].ToString(),
                                                 RaisedDateTime = reader["RaisedDateTime"].ToDateTime(),
                                                 RepeatCount = reader["RepeatCount"].ToInt() + 1
                                             });
                        rowCount++;
                    }

                // Log the amount of alerts that were loaded from the DB
                Logger.LogInfo(String.Format("Successfully loaded '{0}' alerts from the DB.", LoadedAlerts.Count));
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format(
                    "An error occured while loading alerts from the DB: {0}",
                    ex.Message));
            }
        }

        public static void ProcessAlerts()
        {
            if (LoadedAlerts.Count == 0)
                return;

            if (LoadedPatterns.Count == 0)
            {
                Logger.LogError("There are no RxPatterns loaded!");
                return;
            }

            var alertNo = 1;
            for (var i = LoadedAlerts.Count - 1; i >= 0; i--)
            {
                Logger.LogDebug(String.Format(
                    "Processing alert ({0} of {1})",
                    alertNo, LoadedAlerts.Count));

                // attempt to match each alert against the loaded patterns
                foreach (var p in LoadedPatterns.Where(p => p.IsMatch(LoadedAlerts[i].OriginalDescription)))
                    p.ProcessAlert(LoadedAlerts[i]);
                alertNo++;
                if (LoadedAlerts[i].AlertProcessed) continue;

                // could not process the given alert
                DumpUnknownAlert(LoadedAlerts[i].OriginalDescription, LoadedAlerts[i].AlertName);
            }
        }

        public static void ExportAlerts()
        {
            if (LoadedAlerts.Count == 0) return;
            var enabled = Config.GetConfigKeyValue("Alerts.Export.Enabled", false);
            if (!enabled) return;
            var ePath = Config.GetConfigKeyValue("Alerts.Export.Path", @"c:\exportedAlerts.csv");

            var sb = new StringBuilder();
            sb.Append("OriginalDescription,AlertName,Severity,Priority,Category,RaisedDateTime,");
            sb.Append("RepeatCount,LineNo,LinePos,ScomScript,ScriptArgs,WorkflowName,InstanceName,");
            sb.Append("InstanceId,ManagementGroupName,Query,Result,Details,Output,ServerName,Source");
            sb.Append(Environment.NewLine);

            foreach (ScomAlert a in LoadedAlerts)
            {
                sb.Append(a.OriginalDescription.CsvSafeLine());
                sb.Append(a.AlertName.CsvSafeLine());
                sb.Append(a.Severity.CsvSafeLine());
                sb.Append(a.Priority.CsvSafeLine());
                sb.Append(a.Category.CsvSafeLine());
                sb.Append(a.RaisedDateTime.CsvSafeLine());
                sb.Append(a.RepeatCount.CsvSafeLine());
                sb.Append(a.LineNo.CsvSafeLine());
                sb.Append(a.LinePos.CsvSafeLine());
                sb.Append(a.ScomScript.CsvSafeLine());
                sb.Append(a.ScriptArgs.CsvSafeLine());
                sb.Append(a.WorkflowName.CsvSafeLine());
                sb.Append(a.InstanceName.CsvSafeLine());
                sb.Append(a.InstanceId.CsvSafeLine());
                sb.Append(a.ManagementGroupName.CsvSafeLine());
                sb.Append(a.Query.CsvSafeLine());
                sb.Append(a.Result.CsvSafeLine());
                sb.Append(a.Details.CsvSafeLine());
                sb.Append(a.Output.CsvSafeLine());
                sb.Append(a.ServerName.CsvSafeLine());
                sb.Append(a.Source.CsvSafeLine(false));
                sb.Append(Environment.NewLine);
            }

            if(File.Exists(ePath))
                File.Delete(ePath);

            File.WriteAllText(ePath, sb.ToString());
        }


        // =====> Generating the Sql Command
        private static string CreateSqlCommand()
        {
            CurrentQry.Clear();
            BuildSelectStart();

            // decide on the alerts that we want to grab
            if (Config.GetConfigKeyValue("Alerts.SQL.FetchSpecificAlerts", false))
                BuildSelectGetSpecificEvents();
            else
                BuildSelectFilterAlerts();

            BuildSelectOrder();
            BuildSelectDumpSqlCmd();

            return CurrentQry.ToString();
        }

        private static void BuildSelectStart()
        {
            // Options needed for the SQL query
            var selLim = Config.GetConfigKeyValue("Alerts.SQL.LimitSelectTo", 100);
            var selDaysBack = Config.GetConfigKeyValue("Alerts.SQL.SearchDaysBack", 2);
            var selLimSev = Config.GetConfigKeyValue("Alerts.SQL.LimitSeverity", false);
            var selLimSevTo = Config.GetConfigKeyValue("Alerts.SQL.LimitSeverityTo", 0);

            // Build up the basic select statement
            CurrentQry.Append(String.Format("SELECT TOP {0} *{1}", selLim, Environment.NewLine));
            CurrentQry.Append(String.Format("FROM Alert.vAlert{0}", Environment.NewLine));
            CurrentQry.Append(String.Format(
                "WHERE RaisedDateTime >= GETUTCDATE() - {1}{0}",
                Environment.NewLine, selDaysBack));
            if (selLimSev)
                CurrentQry.Append(String.Format("AND Severity = {1}{0}", Environment.NewLine, selLimSevTo));
        }

        private static void BuildSelectFilterAlerts()
        {
            // Ignroe any excluded rules
            if (AlertIgnoreExact.Count > 0)
                foreach (var s in AlertIgnoreExact)
                    CurrentQry.Append(String.Format("AND AlertName != '{1}'{0}", Environment.NewLine, s));

            // ignore loose alerts if there are any
            if (AlertIgnoreLike.Count > 0)
                foreach (var s in AlertIgnoreLike)
                    CurrentQry.Append(String.Format("AND AlertName NOT LIKE '%{1}%'{0}", Environment.NewLine, s));
        }

        private static void BuildSelectGetSpecificEvents()
        {
            if (AlertIncludeExact.Count == 0)
            {
                Logger.LogWarning("Been asked to include exact alerts, however there are no matches defined!");
                return;
            }

            if (AlertIncludeExact.Count == 1)
            {
                // Easiest filter...
                CurrentQry.Append(String.Format("AND AlertName = '{0}'{1}", AlertIncludeExact[0], Environment.NewLine));
                return;
            }
            
            // =======================================================>
            // =====> Filtering like a mofo
            var maxCount = AlertIncludeExact.Count;
            var counter = 1;
            CurrentQry.Append("AND AlertName IN (");

            foreach (string s in AlertIncludeExact)
            {
                CurrentQry.Append(String.Format("'{0}'", s));
                if (counter != maxCount) CurrentQry.Append(", ");
                counter++;
            }

            CurrentQry.Append(String.Format("){0}", Environment.NewLine));
        }

        private static void BuildSelectOrder()
        {
            var sOrderBy = Config.GetConfigKeyValue("Alerts.SQL.OrderBy", "RaisedDateTime");
            var sOrder = Config.GetConfigKeyValue("Alerts.SQL.Order", "DESC");
            CurrentQry.Append(String.Format("ORDER BY {1} {2}{0}",
                                            Environment.NewLine, sOrderBy, sOrder));
        }

        private static void BuildSelectDumpSqlCmd()
        {
            var sDumpCmd = Config.GetConfigKeyValue("Alerts.SQL.DumpCommand", false);
            var sDumpPath = Config.GetConfigKeyValue("Alerts.SQL.DumpCommand.Path", "./current_sql_cmd.sql");
            if (!sDumpCmd) return;

            // drop the file
            if (File.Exists(sDumpPath))
                File.Delete(sDumpPath);
            
            // dump the current command
            if (!File.Exists(sDumpPath))
                File.WriteAllText(sDumpPath, CurrentQry.ToString());
            else
                Logger.LogWarning(String.Format(
                    "Could not dump the SQL Command to '{0}', the file might be open / locked",
                    sDumpPath));
        }

        private static void DumpUnknownAlert(string desc, string name)
        {
            var oDump = Config.GetConfigKeyValue("Alerts.Dev.DumpUnknown", false);
            var oPath = Config.GetConfigKeyValue("Alerts.Dev.DumpPath", "./unknown_alerts/");
            if (!oDump) return;

            IOHelper.CreateDirectory(oPath);
            var fPath = String.Format("{0}{1}.{2}.{3}.{4}.txt",
                                      oPath, DateHelper.GetDate(),
                                      IOHelper.Rand.Next(0, 999), IOHelper.Rand.Next(0, 999),
                                      IOHelper.Rand.Next(0, 999));

            var txt = String.Format("Name: {0}{1}Desc: {2}", name, Environment.NewLine, desc);
            if (File.Exists(fPath)) File.Delete(fPath);
            File.WriteAllText(fPath, txt);
        }


        // =====> Filtering Alerts
        private static void AlertFilterIgnoreExact(string name)
        {
            if (String.IsNullOrEmpty(name)) return;
            if (AlertIgnoreExact.Contains(name)) return;
            AlertIgnoreExact.Add(name);
            Logger.LogDebug(String.Format("Added '{0}' to the 'Ignore Exact' list", name));
        }

        private static void AlertFilterIgnoreLike(string name)
        {
            if (String.IsNullOrEmpty(name)) return;
            if (AlertIgnoreLike.Contains(name)) return;
            AlertIgnoreLike.Add(name);
            Logger.LogDebug(String.Format("Added '{0}' to the 'Ignore Like' list", name));
        }

        private static void AlertFilterIncludeExact(string name)
        {
            if (String.IsNullOrEmpty(name)) return;
            if(AlertIncludeExact.Contains(name)) return;
            AlertIncludeExact.Add(name);
            Logger.LogDebug(String.Format("Added '{0}' to the 'Include Exact' list", name));
        }
    }
}
