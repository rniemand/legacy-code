using System.Collections.Generic;
using Rn.ChatBot.PluginCommon;

namespace Rn.ChatBot
{
    public static class UserFactory
    {
        private static readonly Dictionary<string, ChatUser> Users;
        
        static UserFactory()
        {
            Users = new Dictionary<string, ChatUser>();
        }


        public static ChatUser GetChatUser(string userName)
        {
            if (!Users.ContainsKey(userName))
                Users.Add(userName, new ChatUser(userName));

            return Users[userName];
        }

    }
}
