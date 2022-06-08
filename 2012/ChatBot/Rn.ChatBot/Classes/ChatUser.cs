using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;

namespace Rn.ChatBot.Classes
{
    public class ChatUser
    {
        public Jid UserJid { get; private set; }

        public ChatUser(string userName)
        {
            UserJid = new Jid(userName);
        }

    }
}
