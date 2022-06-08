using System;
using Rn.Core.Logging;
using agsXMPP;
using agsXMPP.protocol.client;

namespace Rn.ChatBot.PluginCommon
{
    public class ChatUser
    {
        public Jid UserJid { get; private set; }

        public ChatUser(string userName)
        {
            UserJid = new Jid(userName);
        }

        public void SendMessage(string msg)
        {
            try
            {
                var reply = new Message
                {
                    Type = MessageType.chat,
                    To = new Jid(UserJid.Bare),
                    Body = msg
                };

                if (ChatBotGlobal.ChatLogOutgoing)
                    Logger.LogDebug(String.Format("[OUT] ({0}): {1}", UserJid.Bare, msg));

                XmppChatBot.ObjXmpp.Send(reply);
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

    }
}
