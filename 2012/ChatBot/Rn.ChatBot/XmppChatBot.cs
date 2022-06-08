using System;
using System.Timers;
using Rn.ChatBot.PluginCommon;
using Rn.Core.Logging;
using agsXMPP;
using agsXMPP.net;
using agsXMPP.protocol.client;

namespace Rn.ChatBot
{
    public static class XmppChatBot
    {
        public static XmppClientConnection ObjXmpp { get; internal set; }
        public static string UserName { get; private set; }
        private static string _userPass;
        public static string ServerAddress { get; private set; }
        private static bool _mustReconnect = false;


        static XmppChatBot()
        {
            try
            {
                // Create the default xmppClientConnection
                ObjXmpp = new XmppClientConnection
                {
                    Resource = "Richard.Chat.API",
                    AutoResolveConnectServer = true
                };

                ObjXmpp.OnLogin += ObjXmpp_OnLogin;
                ObjXmpp.OnMessage += ObjXmpp_OnMessage;

                ObjXmpp.OnSocketError += ObjXmpp_OnSocketError;
                ObjXmpp.OnError += ObjXmpp_OnError;
                ObjXmpp.OnClose += ObjXmpp_OnClose;
                ObjXmpp.OnStreamError += ObjXmpp_OnStreamError;
                ObjXmpp.OnXmppConnectionStateChanged += ObjXmpp_OnXmppConnectionStateChanged;
                ObjXmpp.OnRegisterError += ObjXmpp_OnRegisterError;
                //ObjXmpp.OnPresence += ObjXmpp_OnPresence;


                Locale.LogEvent("chat_bot", "0001");
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        static void ObjXmpp_OnPresence(object sender, Presence pres)
        {
            Logger.LogDebug(String.Format("ObjXmpp_OnPresence: {0}", pres));
        }

        static void ObjXmpp_OnRegisterError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            Logger.LogDebug("ObjXmpp_OnRegisterError");
        }

        static void ObjXmpp_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            Logger.LogDebug(String.Format("ObjXmpp_OnXmppConnectionStateChanged: {0}", state.ToString()));
        }

        static void ObjXmpp_OnStreamError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            Logger.LogDebug(String.Format("ObjXmpp_OnStreamError: {0}", e));
        }

        static void ObjXmpp_OnClose(object sender)
        {
            OpenConnection();
            Logger.LogDebug("ObjXmpp_OnClose");
        }

        static void ObjXmpp_OnError(object sender, Exception ex)
        {
            Logger.LogDebug("ObjXmpp_OnError");
        }

        static void ObjXmpp_OnSocketError(object sender, Exception ex)
        {
            Logger.LogDebug("ObjXmpp_OnSocketError");
        }





        public static void SetUserCredentials(string userName, string userPass)
        {
            UserName = userName;
            _userPass = userPass;
        }

        public static void SetServerAddress(string serverAddress)
        {
            ServerAddress = serverAddress;
        }

        public static void OpenConnection()
        {
            try
            {
                ObjXmpp.Username = UserName;
                ObjXmpp.Password = _userPass;
                ObjXmpp.Server = ServerAddress;
                ObjXmpp.Open();
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
            
        }

        public static void CloseConnection(bool reconnect = false)
        {
            try
            {
                _mustReconnect = reconnect;
                ObjXmpp.Close();
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }


        static void ObjXmpp_OnMessage(object sender, Message msg)
        {
            try
            {
                var user = UserFactory.GetChatUser(msg.From.Bare.ToLower());
                PluginHost.GenerateResponse(user, msg);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        static void ObjXmpp_OnLogin(object sender)
        {
            PluginHost.LoadPlugins();
            Locale.LogEvent("chat_bot", "0002", ServerAddress, UserName);
        }

    }
}
