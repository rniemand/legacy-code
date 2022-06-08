using System;
using System.Text;
using AdslSwitcher2.Classes.Routers;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace AdslSwitcher2.Classes
{
    class AccountSwitcher
    {
        private ADSLAccountStruct AccountInfo;
        private ADSLRouterStruct RouterInfo;
        Logger _logger;

        public AccountSwitcher(ADSLAccountStruct accountInfo, ADSLRouterStruct routerInfo, Logger logger)
        {
            AccountInfo = accountInfo;
            RouterInfo = routerInfo;
            _logger = logger;
        }

        public void SwitchAccount()
        {
            string PostData = string.Empty;
            string PostURL = string.Empty;

            // Get information needed about the router
            Netgear_DG384 RouterSettings = new Netgear_DG384();
            RouterSettings.SetLoginDetails(AccountInfo.UserName, AccountInfo.UserPass);
            PostData = RouterSettings.ToString();
            PostURL = RouterSettings.GetSubmitUrl(RouterInfo.RouterIP);
            byte[] buffer = Encoding.ASCII.GetBytes(PostData);

            //Connect to the router, send login details, prep for post
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(PostURL);
            WebReq.Credentials = new NetworkCredential(RouterInfo.RouterLoginUser, RouterInfo.RouterLoginPass);
            WebReq.Method = "POST";
            WebReq.ContentType = "application/x-www-form-urlencoded";
            WebReq.ContentLength = buffer.Length;
            
            //We open a stream for writing the postvars
            Stream PostDataStream = WebReq.GetRequestStream();
            PostDataStream.Write(buffer, 0, buffer.Length);
            PostDataStream.Close();

            try
            {
                HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                Stream Answer = WebResp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);

                if (WebResp.StatusCode.ToString() == "OK")
                {
                    MessageBox.Show("Account Changed");
                    _logger.WriteEntry("Account Changed", LogLevel.Info);
                }
                else
                {
                    MessageBox.Show("Error changin accounts");
                    _logger.WriteEntry("Error changin accounts", LogLevel.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                _logger.WriteEntry("Error changin accounts", LogLevel.Error);
            }

        }

    }
}
