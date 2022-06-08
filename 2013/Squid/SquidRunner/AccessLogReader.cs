using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using MySql.Data.MySqlClient;
using RnCore.Logging;
using SquidRunner.Classes;
using MySql.Data;
using RnCore.Helpers;

namespace SquidRunner
{
    class AccessLogReader
    {
        public string FileDirectory { get; private set; }
        public string FileName { get; private set; }
        public string FullFilePath { get; private set; }

        private FileStream _fs;
        private StreamReader _sr;
        private long _lastPosition;
        private bool _isReading;
        private MySqlConnection _sqlCon;


        public AccessLogReader(string fileLocation)
        {
            try
            {
                if (!File.Exists(fileLocation))
                {
                    // todo: make this an error
                    RnLogger.Loggers.LogDebug("The file '{0}' could not be found", 100);
                    return;
                }

                // Check that the log file exists
                FullFilePath = fileLocation;
                FileDirectory = Path.GetDirectoryName(fileLocation);
                FileName = Path.GetFileName(fileLocation);
                if (FileDirectory == null || String.IsNullOrEmpty(FileDirectory))
                {
                    // todo: add to logger
                    return;
                }

                // Get our grubby hooks into the log file
                OpenMySqlConnection();
                CreateFileReader();

                var fsw = new FileSystemWatcher(FileDirectory)
                {
                    Filter = FileName,
                    IncludeSubdirectories = false,
                    NotifyFilter = NotifyFilters.Size
                };
                fsw.Changed += fsw_Changed;
                fsw.EnableRaisingEvents = true;

                // todo: add this to the logger
                RnLogger.Loggers.LogDebug("Created our listener", 100);
            }
            catch (Exception ex)
            {
                // todo: add to logger
            }
        }



        private void CreateFileReader()
        {
            try
            {
                _fs = new FileStream(FullFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                _sr = new StreamReader(_fs);
                
                // todo: get this value from the DB
                _fs.Position = _fs.Length;
                _lastPosition = _fs.Length;

                Console.WriteLine("File good for reading");

            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
            }
        }

        private void OpenMySqlConnection()
        {
            try
            {
                const string sqlConStr = "Server=192.168.0.5;Database=squid;Uid=richard;Pwd=...;";
                _sqlCon = new MySqlConnection(sqlConStr);
                _sqlCon.Open();

                if (_sqlCon.State != ConnectionState.Open)
                {
                    Console.WriteLine("Could not open a MySQL connection!");
                    Thread.Sleep(2000);
                    Environment.Exit(-1);
                }
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
            }
        }


        private void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (_isReading) return;

                _isReading = true;

                while (!_sr.EndOfStream)
                {
                    var line = _sr.ReadLine();
                    ProcessLine(line);
                    _lastPosition = _fs.Position;
                }

                _isReading = false;
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
            }
        }

        private void ProcessLine(string line)
        {
            try
            {
                const string rxp = @"^([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+).*?([^\s]+)$";
                var match = Regex.Match(line, rxp, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                var eaC = match.Groups[4].Value.Split('/');
                var ehf = match.Groups[9].Value.Split('/');
                var entry = new AccessLogEntry
                    {
                        Timestamp = WorkDate(match.Groups[1].Value),
                        Elapsed = match.Groups[2].Value.AsInt(),
                        Client = match.Groups[3].Value,
                        Size = match.Groups[5].Value.AsInt(),
                        Method = match.Groups[6].Value,
                        Uri = match.Groups[7].Value,
                        Ident = match.Groups[8].Value,
                        Content = match.Groups[10].Value,
                        Action = eaC[0],
                        Code = eaC[1].AsInt(),
                        Hierarchy = ehf[0],
                        From = ehf[1],
                        BaseUri = WorkBaseUri(match.Groups[7].Value)
                    };

                PushToMySql(entry);
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
            }
        }

        private static string WorkBaseUri(string uri)
        {
            try
            {
                var match = Regex.Match(uri, @"((\w+://|).*?(/|$)).*", RegexOptions.IgnoreCase);
                return match.Groups[1].Value;
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private static DateTime WorkDate(string timestamp)
        {
            try
            {
                if (timestamp.Contains('.'))
                {
                    var parts = timestamp.Split('.');
                    timestamp = parts[0];
                }

                var iTimestamp = timestamp.AsInt();
                var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                dtDateTime = dtDateTime.AddSeconds(iTimestamp).ToLocalTime();
                return dtDateTime;
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
                return DateTime.Now;
            }
        }

        private static string FormatMySqlDateTime(DateTime time)
        {
            try
            {
                return String.Format(
                    "{0}-{1}-{2} {3}:{4}:{5}",
                    time.Year,
                    time.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    time.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    time.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    time.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
                    time.Second.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
                    );
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
                return "";
            }
        }



        private void PushToMySql(AccessLogEntry info)
        {
            try
            {
                const string baseQuery = @"
                INSERT INTO tb_access_raw
                    (`timestamp`,`elapsed`,`client`,`action`,`code`,`size`,`method`,`baseUrl`,`fullUrl`,`ident`,`hierarchy`,`from`,`content`)
                VALUES
                    ('{0}',{1},'{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}')
                ";

                var sqlQuery = String.Format(
                    baseQuery,
                    FormatMySqlDateTime(info.Timestamp),
                    info.Elapsed,
                    info.Client,
                    info.Action,
                    info.Code,
                    info.Size,
                    info.Method,
                    info.BaseUri,
                    info.Uri,
                    info.Ident,
                    info.Hierarchy,
                    info.From,
                    info.Content);

                using (var cmd = new MySqlCommand(sqlQuery, _sqlCon))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine(info.Uri);
            }
            catch (Exception ex)
            {
                // todo: add to logger
                Console.WriteLine(ex.Message);
            }
        }

    }
}
