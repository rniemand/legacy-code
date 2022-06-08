using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RichardTestJson
{
    public class SabLoggerMessage
    {
        public DateTime TimeLogged { get; set; }
        public string Severity { get; set; }
        public string CallingMethod { get; set; }
        public string Message { get; set; }
    }
}
