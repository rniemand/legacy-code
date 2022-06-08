using System;
using System.Xml;
using RnCore.Helpers;
using RnCore.Logging;

namespace Rn.WebManLib.Utils
{
    public class ApiUser
    {
        public string UserName { get; private set; }
        public string ApiKey { get; private set; }
        public bool Enabled { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int ApiCalls { get; private set; }


        public ApiUser()
        {
            // Static constructor
        }

        public ApiUser(string username, string apikey, bool enabled)
        {
            try
            {
                UserName = username.ToLower().Trim();
                ApiKey = apikey;
                Enabled = enabled;
                ApiCalls = 0;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public ApiUser(XmlNode n)
        {
            try
            {
                UserName = n.GetAttributeString("UserName").ToLower().Trim();
                ApiKey = n.GetAttributeString("API");
                Enabled = n.GetAttributeBool("Enabled");
                ApiCalls = n.GetAttributeInt("ApiCalls");
                LastSeen = n.GetAttributeDateTime("LastSeen");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        public void SetEnabledState(bool state)
        {
            Enabled = state;
        }

        public void IncrementApiCallCount()
        {
            try
            {
                ApiCalls++;
                LastSeen = DateTime.Now;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

    }
}
