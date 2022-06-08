using System;
using System.IO;
using System.Xml;
using Rn.Core.Helpers;

namespace Rn.Core.Logging.Outputs
{
    public class LogFile : LogOutput
    {
        public long RollSize { get; private set; }
        public int KeepLogs { get; private set; }
        public bool DebugMode { get; private set; }
        public string LogFilePath { get; private set; }
        public string LogFileFolder { get; private set; }

        private FileStream _fs;
        private StreamWriter _sw;

        public LogFile(string name, LogSeverity sev, XmlNode n)
            : base(name, LoggerType.LogFile, sev)
        {
            try
            {
                KeepLogs = n.GetAttributeInt("Keep", 5);
                RollSize = n.GetAttributeInt("Roll", 5)*1048576;
                DebugMode = n.GetAttributeBool("Debug");
                LogFilePath = n.GetAttribute("File", @"c:\Rn.Core.log");
                LogFileFolder = Path.GetDirectoryName(LogFilePath);

                OpenLogFile();
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Exception creating a new instance of the LogFile logger: {0}", ex.Message));
            }
        }


        // Working with the log file
        private void CloseLogFile()
        {
            if (_sw == null || _fs == null)
                return;

            try
            {
                _sw.Flush();
                _fs.Flush();

                _sw.Close();
                _fs.Close();

                _sw.Dispose();
                _fs.Dispose();
            }
            catch (Exception ex)
            {
                Logger.LogWarning(String.Format("Exception thrown while closing the log file '{0}': {1}", LogFilePath,
                                                ex.Message));
            }
        }

        private void OpenLogFile()
        {
            if (!IOHelper.CreateDir(LogFileFolder))
            {
                Logger.LogError(String.Format("Could not create the log files directory '{0}'", LogFilePath));
                return;
            }

            try
            {
                HandleDebugMode();
                _fs = new FileStream(LogFilePath, FileMode.OpenOrCreate);
                _sw = new StreamWriter(_fs);
                _fs.Position = _fs.Length;
                LoggerReady = true;
                RollLogFile();
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error while opening the file '{0}': {1}", LogFilePath, ex.Message));
            }
        }

        private void HandleDebugMode()
        {
            if (DebugMode && File.Exists(LogFilePath))
                IOHelper.DeleteFile(LogFilePath);
        }

    private void RollLogFile()
        {
            if (_fs.Length < RollSize)
                return;

            LoggerReady = false;
            CloseLogFile();
            ShiftLogFiles();
            OpenLogFile();
        }

        private void ShiftLogFiles()
        {
            // Shift all files up 1 position
            for (var i = KeepLogs; i > 0; i--)
            {
                var curName = String.Format("{0}.{1}", LogFilePath, i);
                var newName = String.Format("{0}.{1}", LogFilePath, i + 1);

                if (i == KeepLogs && File.Exists(curName))
                {
                    File.Delete(curName);
                    continue;
                }

                if (File.Exists(curName))
                    IOHelper.RenameFile(curName, newName);
            }

            // Rename the original file
            var oNewName = String.Format("{0}.{1}", LogFilePath, 1);
            IOHelper.RenameFile(LogFilePath, oNewName);
        }

        private void WriteToLog(string msg)
        {
            try
            {
                RollLogFile();

                _sw.WriteLine(msg);
                _sw.Flush();
            }
            catch (Exception ex)
            {
                Logger.LogError(String.Format("Error trying to write a new message to the log file '{0}': {1}",
                                              LogFilePath, ex.Message));
            }
        }

        private void WriteToLogFile(string msg, LogSeverity sev, int eventId, bool fromLocale = false)
        {
            var framesback = fromLocale ? 6 : 4;

            if (eventId == 0)
                WriteToLog(String.Format(
                    "[{0}] ({1}) {2} : {3}",
                    DateHelper.GetTimeString(),
                    GetFullMethodPath(framesback),
                    sev.AsString().ToUpper(),
                    msg));
            else
                WriteToLog(String.Format(
                    "[{0}] ({4}) ({1}) {2} : {3}",
                    DateHelper.GetTimeString(),
                    GetFullMethodPath(framesback),
                    sev.AsString().ToUpper(),
                    msg,
                    eventId));
        }


        // Logging methods
        public override void LogDebug(string msg, int eventId = 0, bool fromLocale = false)
        {
            WriteToLogFile(msg, LogSeverity.Debug, eventId, fromLocale);
        }

        public override void LogInfo(string msg, int eventId = 0, bool fromLocale = false)
        {
            WriteToLogFile(msg, LogSeverity.Info, eventId, fromLocale);
        }

        public override void LogWarning(string msg, int eventId = 0, bool fromLocale = false)
        {
            WriteToLogFile(msg, LogSeverity.Warning, eventId, fromLocale);
        }

        public override void LogError(string msg, int eventId = 0, bool fromLocale = false)
        {
            WriteToLogFile(msg, LogSeverity.Error, eventId, fromLocale);
        }

    }
}
