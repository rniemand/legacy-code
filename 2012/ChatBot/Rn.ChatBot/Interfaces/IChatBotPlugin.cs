using System;
using Rn.ChatBot.PluginCommon;
using agsXMPP.protocol.client;

namespace Rn.ChatBot.Interfaces
{
    public interface IChatBotPlugin
    {
        string PluginName { get; }
        Version PluginVersion { get; }

        void LoadPlugin();
        void PluginCallback(ChatUser usr, Message msg, CallbackInfo cb);
    }
}
