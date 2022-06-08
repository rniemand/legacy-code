using System;
using System.IO;
using System.Xml;
using RnCore.Helpers;

namespace RnCore.Logging.Outputs
{
    class LogFile : LoggerBase
    {
        public string LogFileDirectory { get; private set; }
        public string LogFileLocation { get; private set; }
        public int Keep { get; private set; }
        public long MaxSize { get; private set; }

        private FileStream _logFs;
        private StreamWriter _logSw;


        public LogFile(XmlNode n)
            : base(n, LoggerType.LogFile)
        {
            try
            {
                // Set internal log file properties
                Keep = n.GetAttrInt("KeepCount", 5);
                LogFileLocation = n.GetAttr("Location").MakeRelative();
                MaxSize = (long)Math.Floor(n.GetAttrDbl("MaxSize", 10) * 1048576);
                LogFileDirectory = Path.GetDirectoryName(LogFileLocation);

                CheckLoggingDir();
                OpenLogFile();
                CheckLogSize();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


        // =====> Setup Methods
        private void CheckLoggingDir()
        {
            if (!RnIO.CreateDirectory(LogFileDirectory))
            {
                RnLocale.LogEvent("rn", "rn.0006", LogFileDirectory);
                return;
            }
        }

        private void OpenLogFile()
        {
            try
            {
                // attempt to create/open the log file
                _logFs = new FileStream(LogFileLocation, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                _logSw = new StreamWriter(_logFs);
                _logFs.Position = _logFs.Length;
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        private void CloseLogFile()
        {
            try
            {
                _logSw.Flush();
                _logFs.Flush();

                _logSw.Dispose();
                _logFs.Dispose();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }

        private void CheckLogSize()
        {
            if (_logFs.Position < MaxSize)
                return;

            CloseLogFile();
            RollLogFile();
            OpenLogFile();
        }

        private void RollLogFile()
        {
            try
            {
                // Shift all files over by 1 place
                for (var i = Keep; i > 0; i--)
                {
                    var tmpCur = String.Format("{0}.{1}", LogFileLocation, i);
                    var tmpNew = String.Format("{0}.{1}", LogFileLocation, i + 1);

                    if (i == Keep && File.Exists(tmpCur))
                        RnIO.Delete(tmpCur);
                    else if (File.Exists(tmpCur))
                        RnIO.Move(tmpCur, tmpNew, true);
                }

                // Rename the original file
                RnIO.Move(LogFileLocation, String.Format("{0}.1", LogFileLocation));
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


        // =====> Logging methods
        public override void LogDebug(string message, int eventId = 0)
        {
            WriteToLog(FormatMessage(message, eventId, LoggerSeverity.Debug));
        }

        public override void LogInfo(string message, int eventId = 0)
        {
            WriteToLog(FormatMessage(message, eventId, LoggerSeverity.Informational));
        }

        public override void LogWarning(string message, int eventId = 0)
        {
            WriteToLog(FormatMessage(message, eventId, LoggerSeverity.Warning));
        }

        public override void LogError(string message, int eventId = 0)
        {
            WriteToLog(FormatMessage(message, eventId, LoggerSeverity.Error));
        }


        // =====> Misc Methods
        private void WriteToLog(string message)
        {
            try
            {
                CheckLogSize();
                _logSw.WriteLine(message);
                _logSw.Flush();
                _logSw.Flush();
            }
            catch (Exception ex)
            {
                RnLocale.LogEvent("rn.common", "common.001", ex.Message);
            }
        }


    }
}
