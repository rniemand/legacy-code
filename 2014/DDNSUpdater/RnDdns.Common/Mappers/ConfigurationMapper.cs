using System.Xml;
using RnDdns.Common.Enums;
using RnDdns.Common.Helpers;
using RnDdns.Common.Models;

namespace RnDdns.Common.Mappers
{
    public static class ConfigurationMapper
    {
        public static RnConfigCredentials MapCredentials(XmlNode node)
        {
            var mapped = new RnConfigCredentials();

            // todo: null checks...
            var id = node.Attributes["id"].Value;
            var username = node.Attributes["username"].Value;
            var password = EncryptionHelper.Base64Decode(node.Attributes["password"].Value);

            // Map
            mapped.Id = id;
            mapped.Password = password;
            mapped.Username = username;

            return mapped;
        }

        public static RnConfigDomain MapDomain(XmlNode node)
        {
            var mapped = new RnConfigDomain();

            // todo: null checks...
            var id = int.Parse(node.Attributes["id"].Value);
            var type = DomainType.DDNS; // todo: resolve from name
            var enabled = bool.Parse(node.Attributes["enabled"].Value);
            var serverUrl = node.Attributes["serverUrl"].Value;
            var hostname = node.Attributes["hostname"].Value;
            var credentialsId = node.Attributes["credentialsId"].Value;

            // map the object
            mapped.CredentialsId = credentialsId;
            mapped.Enabled = enabled;
            mapped.Hostname = hostname;
            mapped.Id = id;
            mapped.ServerUrl = serverUrl;
            mapped.Type = type;

            return mapped;
        }
    }
}
