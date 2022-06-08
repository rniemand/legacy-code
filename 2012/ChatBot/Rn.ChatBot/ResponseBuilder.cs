using System;
using Rn.ChatBot.PluginCommon;
using Rn.Core.Logging;
using agsXMPP.protocol.client;

namespace Rn.ChatBot
{
    public static class ResponseBuilder
    {

        public static void LoadResponseModules()
        {
            
        }


        public static void GenerateResponse(ChatUser usr, Message msg)
        {
            if (ChatBotGlobal.ChatLogIncomming)
                Logger.LogDebug(String.Format("[IN] ({0}): {1}", msg.From.Bare, msg.Body));

            var resp = false;
            var cb = PluginHost.GetCallback(msg.Body);

            if (cb != null)
                resp = PluginHost.RunCallback(usr, msg, cb);

            if (ChatBotGlobal.ChatLogOutgoing)
                Logger.LogDebug(String.Format("[OUT] ({0}): {1}", msg.From.Bare, resp));

            // todo - add handler for no response here
        }

    }
}
