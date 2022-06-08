using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rn.WebManLib.Externals.EventLog
{
    public class SoapEventLog
    {
        public bool EnableRaisingEvents { get; set; }
        public string Log { get; set; }
        public string LogDisplayName { get; set; }
        public string MachineName { get; set; }
        public long MaximumKilobytes { get; set; }
        public int MinimumRetentionDays { get; set; }
        public string OverflowAction { get; set; }
        public string Source { get; set; }
    }
}
