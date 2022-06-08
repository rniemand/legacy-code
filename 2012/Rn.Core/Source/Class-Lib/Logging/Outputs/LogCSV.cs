using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Core.Logging.Outputs
{
    public class LogCSV : LogOutput
    {
        public LogCSV(string name, LogSeverity sev)
            : base(name, LoggerType.CSV, sev)
        {
            // holder for now
        }
    }
}
