using System;
using System.Text.RegularExpressions;
using Rn.ChatBot.PluginCommon;
using Rn.ChatBot.Plugins.TestPlugin.adsl;
using agsXMPP.protocol.client;

namespace Rn.ChatBot.Plugins.TestPlugin
{
    public class TestPlugin : IChatBotPlugin
    {
        public string PluginName { get; private set; }
        public Version PluginVersion { get; private set; }
        private readonly Random _r = new Random();

        public TestPlugin()
        {
            PluginName = "Test Plugin";
            PluginVersion = new Version(1, 0, 0);
        }



        public void LoadPlugin()
        {
            PluginHost.RegisterPhrase(new CallbackInfo("hi", PluginName, "fn001"));
            PluginHost.RegisterPhrase(new CallbackInfo(".*?(random number)", PluginName, "fn002"));
            PluginHost.RegisterPhrase(new CallbackInfo("adsl-acc.*", PluginName, "fn003"));
        }

        public void PluginCallback(ChatUser usr, Message msg, CallbackInfo cb)
        {
            switch (cb.FunctionName)
            {
                case "fn001": Fn001(usr, msg);
                    break;
                case "fn002":
                    Fn002(usr, msg);
                    break;
                case "fn003":
                    Fn003(usr, msg);
                    break;
                default:
                    UnknownCallback(usr, msg);
                    break;
            }
        }

        private static void UnknownCallback(ChatUser usr, Message msg)
        {
            usr.SendMessage("Unknown callback");
        }

        private static void Fn001(ChatUser usr, Message msg)
        {
            usr.SendMessage("Hello World");
        }

        private void Fn002(ChatUser usr, Message msg)
        {
            usr.SendMessage(_r.Next(1, 9999).ToString());
        }

        private void Fn003(ChatUser usr, Message msg)
        {
            try
            {
                var rxp = "adsl-acc( |)(.*)";
                var match = Regex.Match(msg.Body, rxp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var accName = match.Groups[2].Value.Trim().ToLower();
                
                switch (accName)
                {
                    case "fnb":
                        usr.SendMessage(String.Format("Switching ADSL account to: {0}", "..."));
                        AccountSwitcher.SwitchAccount("RNIEMAND@fnbconnect.co.za", "...");
                        break;
                    case "mweb":
                        usr.SendMessage(String.Format("Switching ADSL account to: {0}", "..."));
                        AccountSwitcher.SwitchAccount("m9721710@mweb.co.za", "...");
                        break;
                    case "ah1":
                        usr.SendMessage(String.Format("Switching ADSL account to: {0}", "..."));
                        AccountSwitcher.SwitchAccount("richardniemand2@afrihost.co.za", "...");
                        break;
                    case "ah2":
                        usr.SendMessage(String.Format("Switching ADSL account to: {0}", "..."));
                        AccountSwitcher.SwitchAccount("richardniemand4@afrihost.co.za", "...");
                        break;
                    default:
                        usr.SendMessage(String.Format("Unknown ADSL Account: '{0}'", accName));
                        break;
                }
            }
            catch (Exception ex)
            {
                usr.SendMessage(String.Format("Error setting the ADSL account: {0}", ex.Message));
            }

            //AccountSwitcher.SwitchAccount();
        }

    }
}
