using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Rn.Core.Configuration;
using Rn.Core.Helpers;
using Rn.Core.Logging;
using agsXMPP.protocol.client;

namespace Rn.ChatBot.PluginCommon
{
    public static class PluginHost
    {
        private static int _pluginCount;
        private static readonly List<IChatBotPlugin> Plugins;
        private static readonly List<CallbackInfo> Callbacks;
        private static readonly string PluginDir;

        static PluginHost()
        {
            Plugins = new List<IChatBotPlugin>();
            Callbacks = new List<CallbackInfo>();
            _pluginCount = 0;
            PluginDir = Config.GetXmlConfig().GetConfigKey("Plugins.Dir").MakeRelativePath();

            if (!Directory.Exists(PluginDir))
                IOHelper.CreateDir(PluginDir);
        }


        // Loading of plugins
        public static void LoadPlugins()
        {
            try
            {
                if (!Directory.Exists(PluginDir))
                {
                    Locale.LogEvent("chat_bot", "0004", PluginDir);
                    return;
                }

                var di = new DirectoryInfo(PluginDir);
                var plugins = di.GetFiles("*.dll");

                if (plugins.Length == 0)
                {
                    Locale.LogEvent("chat_bot", "0005", PluginDir);
                    return;
                }

                foreach (var p in plugins)
                    LoadPluginDll(p);

                Locale.LogEvent("chat_bot", "0006");
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }

        private static void LoadPluginDll(FileInfo dll)
        {
            try
            {
                const string interfaceName = "Rn.ChatBot.PluginCommon.IChatBotPlugin";
                var pluginAssembly = Assembly.LoadFrom(dll.FullName);

                foreach (var pType in pluginAssembly.GetTypes())
                {
                    if (!pType.IsPublic || pType.IsAbstract) continue;
                    var typeInterface = pType.GetInterface(interfaceName, true);
                    if (typeInterface == null) continue;

                    try
                    {
                        var p = (IChatBotPlugin)Activator.CreateInstance(pluginAssembly.GetType(pType.ToString()));
                        p.LoadPlugin();
                        Plugins.Add(p);
                        Locale.LogEvent("chat_bot", "0007", p.PluginName, p.PluginVersion, pType.Namespace,
                                        pType.Assembly);
                    }
                    catch (Exception ex)
                    {
                        Locale.LogEvent("rn.core.common", "0002", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
            }
        }



        public static void RegisterPhrase(CallbackInfo cb)
        {
            Locale.LogEvent("chat_bot", "0008", cb.PluginName, cb.RxPattern, cb.FunctionName);
            Callbacks.Add(cb);
        }

        public static CallbackInfo GetCallback(string msg)
        {
            if (Callbacks.Count == 0 || String.IsNullOrEmpty(msg))
                return null;

            foreach (CallbackInfo c in Callbacks)
            {
                if (Regex.IsMatch(msg, c.RxPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    return c;
            }

            return null;
        }

        public static bool HasPLugin(string pluginName)
        {
            return Plugins.Count != 0 && Plugins.Any(p => p.PluginName == pluginName);
        }

        public static IChatBotPlugin GetPlugin(CallbackInfo cb)
        {
            return GetPluginByName(cb.PluginName);
        }

        public static IChatBotPlugin GetPluginByName(string pluginName)
        {
            return Plugins.Count == 0 ? null : Plugins.FirstOrDefault(p => p.PluginName == pluginName);
        }



        // Execution of plugin callbacks
        public static void GenerateResponse(ChatUser usr, Message msg)
        {
            if (ChatBotGlobal.ChatLogIncomming)
                Logger.LogDebug(String.Format("[IN] ({0}): {1}", msg.From.Bare, msg.Body));

            var resp = false;
            var cb = GetCallback(msg.Body);

            if (cb != null) resp = RunCallback(usr, msg, cb);
            if (resp) return;

            Locale.LogEvent("chat_bot", "0010", msg.Body, usr.UserJid.Bare);
            usr.SendMessage("Unknown command...");
        }

        public static bool RunCallback(ChatUser usr, Message msg, CallbackInfo cb)
        {
            if(!HasPLugin(cb.PluginName))
            {
                Locale.LogEvent("chat_bot", "0009", cb.PluginName);
                return false;
            }

            try
            {
                var p = GetPlugin(cb);
                p.PluginCallback(usr, msg, cb);
                return true;
            }
            catch (Exception ex)
            {
                Locale.LogEvent("rn.core.common", "0002", ex.Message);
                return false;
            }
        }

    }
}
