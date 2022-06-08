using System;

namespace RnDdns.Common.Models
{
    public class UserDomain
    {
        public string ServerUrl { get; set; }
        public string HostName { get; set; }
        public string LastIp { get; set; }
        public DateTime LastUpdated { get; set; }
        public UserCredentials Creds { get; set; }
    }
}
