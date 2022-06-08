using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SquidRunner.Classes
{
    class AccessLogEntry
    {
        public DateTime Timestamp { get; set; }
        public int Elapsed { get; set; }
        public string Client { get; set; }
        public string Action { get; set; }
        public int Code { get; set; }
        public int Size { get; set; }
        public string Method { get; set; }
        public string Uri { get; set; }
        public string BaseUri { get; set; }
        public string Ident { get; set; }
        public string Hierarchy { get; set; }
        public string From { get; set; }
        public string Content { get; set; }
    }
}
