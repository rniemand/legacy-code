using agsXMPP.protocol.client;

namespace Rn.ChatBot.PluginCommon
{
    public class CallbackInfo
    {
        public string RxPattern { get; private set; }
        public string PluginName { get; private set; }
        public string FunctionName { get; private set; }
        public Message LastMessage { get; set; }

        public CallbackInfo(string rxPattern, string pluginName, string fnName)
        {
            RxPattern = rxPattern;
            PluginName = pluginName;
            FunctionName = fnName;
        }

    }
}
