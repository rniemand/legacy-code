using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Rn.Helpers;

namespace Rn.Logging.Outputs
{
    public class OutLogFile : LoggerOutput
    {
        public string LogFilePath { get; internal set; }
        public int LogFileRollSizeMb { get; internal set; }
        public int LogsToKeep { get; internal set; }

        private bool _canLog;
        private readonly long _rollSize;
        private FileStream _fs;
        private StreamWriter _sw;

        // =====> Class Constructor
        public OutLogFile(string path, int maxLogSize = 10, int keepLogFiles = 10, string name = "default", Severity lvl = Severity.Debug)
            : base(name, lvl, LoggerType.LogFile)
        {
            // Map basic information
            LogFilePath = path;
            LogFileRollSizeMb = maxLogSize;
            LogsToKeep = keepLogFiles;
            _canLog = false;
            _rollSize = (maxLogSize*1048576);

            OpenLogFile();
        }


        // =====> Working With The Log File
        private void OpenLogFile()
        {
            try
            {
                // Open the log file and position the cursor at the EOF
                _fs = new FileStream(LogFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                _fs.Position = _fs.Length;
                _sw = new StreamWriter(_fs);
                _canLog = true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex.Message);
            }
        }

        private void CloseLogFile()
        {
            if (_fs == null || _sw == null)
                return;

            try
            {
                _sw.Flush();
                _fs.Flush();
                _sw.Close();
                _fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // todo - add exception here
            }
        }

        private void WriteLine(string msg, string callingMethod, Severity sev, int eventId = 0)
        {
            if (!_canLog) return;

            if (_fs.Length >= _rollSize)
                RollLogFile();

            var sb = new StringBuilder();
            sb.Append(String.Format(
                "[{0}] [{1}] {2}",
                DateHelper.GetFullDate(),
                callingMethod,
                sev.ToString().ToUpper())
                );
            if (eventId != 0) sb.Append(String.Format(" ({0})", eventId));
            sb.Append(": " + msg);

            _sw.WriteLine(sb.ToString());
            _fs.Flush();
            _sw.Flush();
        }

        private void RollLogFile()
        {
            CloseLogFile();

            for (var i = LogsToKeep; i >= 1; i--)
            {
                var curName = String.Format("{0}.{1}", LogFilePath, i);
                var newName = String.Format("{0}.{1}", LogFilePath, i + 1);

                if (i == LogsToKeep && File.Exists(curName))
                {
                    File.Delete(curName);
                    continue;
                }

                if (File.Exists(curName))
                    File.Move(curName, newName);
            }

            if (File.Exists(LogFilePath))
                File.Move(LogFilePath, String.Format("{0}.1", LogFilePath));

            OpenLogFile();
        }


        // =====> Writing Events to the Log File
        public override void LogError(string message, string callingMethod, int eventId = 0)
        {
            WriteLine(message, callingMethod, Severity.Error, eventId);
        }

        public override void LogWarning(string message, string callingMethod, int eventId = 0)
        {
            WriteLine(message, callingMethod, Severity.Warning, eventId);
        }

        public override void LogInfo(string message, string callingMethod, int eventId = 0)
        {
            WriteLine(message, callingMethod, Severity.Information, eventId);
        }

        public override void LogDebug(string message, string callingMethod, int eventId = 0)
        {
            WriteLine(message, callingMethod, Severity.Debug, eventId);
        }

    }
}
