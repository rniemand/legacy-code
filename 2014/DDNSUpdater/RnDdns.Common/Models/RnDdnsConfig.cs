using System.Collections.Generic;
using RnDdns.Common.Enums;

namespace RnDdns.Common.Models
{
    public class RnDdnsConfig
    {
        public List<RnConfigCredentials> Credentials { get; set; }
        public List<RnConfigDomain> Domains { get; set; }

        public RnDdnsConfig()
        {
            Credentials = new List<RnConfigCredentials>();
            Domains = new List<RnConfigDomain>();
        }
    }

    public class RnConfigCredentials
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RnConfigDomain
    {
        public int Id { get; set; }
        public DomainType Type { get; set; }
        public string ServerUrl { get; set; }
        public string Hostname { get; set; }
        public bool Enabled { get; set; }
        public string CredentialsId { get; set; }
        public RnConfigCredentials Credentials { get; set; }
    }
}
