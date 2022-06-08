using System;
using System.Text;
using System.Threading;
using AdslSwitcher2.Classes;
using AdslSwitcher2.Classes.Routers;
using System.Net;
using System.IO;
using Rn.Core.Logging;

namespace Rn.ChatBot.Plugins.TestPlugin.adsl
{
    static class AccountSwitcher
    {

        public static void SwitchAccount(string accUser, string accPass)
        {
            XmppChatBot.CloseConnection();

            // Get information needed about the router
            var routerSettings = new Netgear_DG384();
            routerSettings.SetLoginDetails(accUser, accPass);
            var postData = routerSettings.ToString();
            var postURL = routerSettings.GetSubmitUrl("192.168.0.1");
            byte[] buffer = Encoding.ASCII.GetBytes(postData);

            //Connect to the router, send login details, prep for post
            var webReq = (HttpWebRequest)WebRequest.Create(postURL);
            webReq.Credentials = new NetworkCredential("admin", "...");
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = buffer.Length;
            
            //We open a stream for writing the postvars
            var postDataStream = webReq.GetRequestStream();
            postDataStream.Write(buffer, 0, buffer.Length);
            postDataStream.Close();

            try
            {
                var webResp = (HttpWebResponse)webReq.GetResponse();

                if (webResp.StatusCode.ToString() == "OK")
                {
                    Console.WriteLine("success");
                }
                else
                {
                    Console.WriteLine("error switching accounts...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Wait 20 seconds and then send a reconnect
            Logger.LogDebug("Sleeping 30 seconds before reconnection attempt");
            Thread.Sleep(30000);
            Logger.LogDebug("Attempting to reconnect XMPP");
            XmppChatBot.OpenConnection();
        }

    }
}
