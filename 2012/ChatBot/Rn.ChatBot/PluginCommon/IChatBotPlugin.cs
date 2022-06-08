using System;
using agsXMPP.protocol.client;

namespace Rn.ChatBot.PluginCommon
{
    public interface IChatBotPlugin
    {
        string PluginName { get; }
        Version PluginVersion { get; }

        void LoadPlugin();
        void PluginCallback(ChatUser usr, Message msg, CallbackInfo cb);
    }
}
