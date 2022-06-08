using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdslSwitcher2.Classes
{
    class Logger
    {
        ToolStripStatusLabel _target;

        public Logger(ToolStripStatusLabel lable)
        {
            _target = lable;
        }

        public void WriteEntry(string message, LogLevel level)
        {
            _target.Text = message;
        }

        public void WriteToLog(string message, LogLevel level)
        {

        }

    }

    enum LogLevel
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Debug = 4
    }
}
