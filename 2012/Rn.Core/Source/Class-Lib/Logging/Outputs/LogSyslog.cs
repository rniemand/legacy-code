using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Core.Logging.Outputs
{
    public class LogSyslog : LogOutput
    {

        public LogSyslog(string name, LogSeverity sev)
            : base(name, LoggerType.Syslog, sev)
        {
            // holder for now
        }

    }
}
