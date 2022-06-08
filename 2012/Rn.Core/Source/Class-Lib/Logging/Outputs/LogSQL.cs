using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rn.Core.Logging.Outputs
{
    public class LogSQL : LogOutput
    {

        public LogSQL(string name, LogSeverity sev)
            : base(name, LoggerType.SQL, sev)
        {
            // holder for now
        }

    }
}
