using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.Core.MySQL
{
    public class MySqlConnectionString
    {
        public string ServerAddress { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool UseEncryption { get; set; }
        public bool ConnectionEnabled { get; set; }
        public string ConnectionName { get; set; }
        public bool AutoOpen { get; set; }


        // Class Constructors
        public MySqlConnectionString()
        {
            SetDefaults();
        }

        public MySqlConnectionString(XmlNode n)
        {
            try
            {
                SetDefaults();
                ConnectionEnabled = n.GetAttributeBool("Enabled");
                ServerAddress = n.GetAttribute("Host");
                Database = n.GetAttribute("DB");
                UserName = n.GetAttribute("User");
                Password = n.GetAttribute("Pass");
                ConnectionName = n.GetAttribute("Name");
                AutoOpen = n.GetAttributeBool("AutoOpen");
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        private void SetDefaults()
        {
            ConnectionEnabled = true;
            ConnectionName = "Default";
        }

        // Get the connection string
        public string GetConnectionString()
        {
            var conStr = new StringBuilder();

            conStr.Append(String.Format("Server={0};", ServerAddress));
            
            if (Port > 0)
                conStr.Append(String.Format("Port={0};", Port));
            
            conStr.Append(String.Format("Database={0};", Database));
            
            if (UserName.Length > 0)
                conStr.Append(String.Format("Uid={0};Pwd={1};", UserName, Password));

            if (UseEncryption)
                conStr.Append("Encryption=true;");

            return conStr.ToString();
        }
    }
}
