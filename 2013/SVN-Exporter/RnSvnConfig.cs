using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvnTest
{
    class RnSvnConfig
    {
        public string SvnUri { get; set; }
        public string DirCheckout { get; set; }
        public string DirExport { get; set; }
        public bool ZipEnabled { get; set; }
    }
}
