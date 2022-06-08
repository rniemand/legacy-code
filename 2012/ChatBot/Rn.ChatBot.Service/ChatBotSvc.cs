using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Rn.Core.Configuration;
using Rn.Core.Helpers;
using Rn.Core.Logging;

namespace Rn.ChatBot.Service
{
    public partial class ChatBotSvc : ServiceBase
    {
        #region Required Controls
        public ChatBotSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        protected override void OnStop()
        {
            StopService();
        } 
        #endregion

        
        public void StartService()
        {
            LoadConfigFile();
            CreateChatBot();
        }

        public void StopService()
        {
            
        }


        // Service startup methods
        private static void LoadConfigFile()
        {
            Config.LoadXmlConfig("ChatConfig", "./ChatConfig.xml");
            var tmpConfig = Config.GetXmlConfig();
            tmpConfig.CreateLoggers();

            ChatBotGlobal.ChatLogIncomming = tmpConfig.GetConfigKeyBool("Messages.Log.Incomming");
            ChatBotGlobal.ChatLogOutgoing = tmpConfig.GetConfigKeyBool("Messages.Log.Outgoing");
        }

        private static void CreateChatBot()
        {
            try
            {
                var accouts = Config.GetXmlConfig().SelectNodes("/ChatConfig/ChatAccounts/Account[@Enabled='true']");

                if (accouts.Count > 0)
                {
                    XmppChatBot.SetServerAddress(accouts[0].GetAttribute("Server"));
                    XmppChatBot.SetUserCredentials(accouts[0].GetAttribute("UserName"), accouts[0].GetAttribute("UserPass"));
                    XmppChatBot.OpenConnection();
                }
                else
                {
                    Locale.LogEvent("chat_bot", "0003");
                }
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }


    }
}
