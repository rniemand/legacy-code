using System.Text;

namespace AdslSwitcher2.Classes.Routers
{
    class Netgear_DG384
    {
        // Public Properties
        public string DSLencapsulation { get; set; }
        public string pppoeService { get; set; }
        public string WAN_ipType { get; set; }
        public string DNStype { get; set; }
        public string natEnable { get; set; }
        public string apply { get; set; }
        public string h_DSLencapsulation { get; set; }
        public string wan_login { get; set; }
        public string h_natEnable { get; set; }
        public string h_WANlogin { get; set; }
        public string h_WAN_ipType { get; set; }
        public string c4_pppoeip { get; set; }
        public string h_DNStype { get; set; }
        public string c4_DNS1address { get; set; }
        public string c4_DNS2address { get; set; }
        public string h_pppoeRelay_enable { get; set; }
        public string runtest { get; set; }
        public string todo { get; set; }
        public string this_file { get; set; }
        public string next_file { get; set; }
        public int pppoeIdleTime { get; set; }

        // Private Properties
        private string pppoeName = string.Empty;
        private string pppoePasswd = string.Empty;

        public Netgear_DG384()
        {
            DSLencapsulation = "pppoe";
            pppoeService = "";
            pppoeIdleTime = 0;
            WAN_ipType = "Dynamic";
            DNStype = "Dynamic";
            natEnable = "enabled";
            apply = "Apply";
            h_DSLencapsulation = "pppoe";
            wan_login = "setup.cgi%3Fnext_file%3Dpppoe.htm";
            h_natEnable = "enabled";
            h_WANlogin = "enable";
            h_WAN_ipType = "Dynamic";
            c4_pppoeip = "";
            h_DNStype = "Dynamic";
            c4_DNS1address = "";
            c4_DNS2address = "";
            h_pppoeRelay_enable = "disable";
            runtest = "";
            todo = "save";
            this_file = "pppoe.htm";
            next_file = "basic.htm";
        }

        public void SetLoginDetails(string accountName, string accountPass)
        {
            pppoeName = accountName;
            pppoePasswd = accountPass;
        }

        public override string ToString()
        {
            var returnString = new StringBuilder();

            returnString.Append("DSLencapsulation=" + DSLencapsulation);
            returnString.Append("&pppoeName=" + pppoeName);
            returnString.Append("&pppoePasswd=" + pppoePasswd);
            returnString.Append("&pppoeService=" + pppoeService);
            returnString.Append("&pppoeIdleTime=" + pppoeIdleTime);
            returnString.Append("&WAN_ipType=" + WAN_ipType);
            returnString.Append("&DNStype=" + DNStype);
            returnString.Append("&natEnable=" + natEnable);
            returnString.Append("&apply=" + apply);
            returnString.Append("&h_DSLencapsulation=" + h_DSLencapsulation);
            returnString.Append("&wan_login=" + wan_login);
            returnString.Append("&h_natEnable=" + h_natEnable);
            returnString.Append("&h_WANlogin=" + h_WANlogin);
            returnString.Append("&h_WAN_ipType=" + h_WAN_ipType);
            returnString.Append("&c4_pppoeip=" + c4_pppoeip);
            returnString.Append("&h_DNStype=" + h_DNStype);
            returnString.Append("&c4_DNS1address=" + c4_DNS1address);
            returnString.Append("&c4_DNS2address=" + c4_DNS2address);
            returnString.Append("&h_pppoeRelay_enable=" + h_pppoeRelay_enable);
            returnString.Append("&runtest=" + runtest);
            returnString.Append("&todo=" + todo);
            returnString.Append("&this_file=" + this_file);
            returnString.Append("&next_file=" + next_file);

            return returnString.ToString();
        }

        public string GetSubmitUrl(string IPAddress)
        {
            var returnString = new StringBuilder();
            returnString.Append("http://<IPADD>/setup.cgi");
            returnString.Replace("<IPADD>", IPAddress);
            return returnString.ToString();
        }

    }
}
