using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdslSwitcher2.Classes
{
    public struct ADSLAccountStruct
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public bool DefaultAccount { get; set; }
    }
}
