using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RnDdns.Common.Models;

namespace RnDdns.Common.Helpers
{
    public class DdnsWorker
    {
        private static readonly Lazy<DdnsWorker> Lazy = new Lazy<DdnsWorker>(() => new DdnsWorker());

        public static DdnsWorker Instance { get { return Lazy.Value; } }

        public DdnsWorker()
        {
            // holder
        }

        private DateTime _lastIpCheckTime;
        private string _lastIp;

        public bool UpdateIpAddress(UserDomain domain)
        {
            RefreshIpAddress();

            if (domain.LastIp == _lastIp)
            {
                RnLogger.Instance.Debug(string.Format(
                    "Skipping update for '{0}', IP address is the same as last time '{1}'",
                    domain.HostName, _lastIp));

                return false;
            }

            if (string.IsNullOrWhiteSpace(_lastIp))
            {
                RnLogger.Instance.Warn(string.Format(
                    "Skipping IP Address update for '{0}' as the IP Address is NULL",
                    domain.HostName));

                return false;
            }

            var updateUrl = domain.ServerUrl + "/client/update.aspx?host=" + domain.HostName + "&myip=" + _lastIp + "";

            RnLogger.Instance.Debug(string.Format(
                "Attempting to update '{0}' ({1})",
                domain.HostName, updateUrl));

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = CustomCertValidation;
                var networkCredential = new NetworkCredential(domain.Creds.Username, domain.Creds.Password);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(updateUrl);
                httpWebRequest.UserAgent = "DDNS-Enterprise-Client-v2.0";
                httpWebRequest.Method = "GET";
                httpWebRequest.Credentials = (ICredentials)networkCredential;
                Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
                var serverResponse = new StreamReader(responseStream).ReadToEnd();
                responseStream.Close();

                // ensure that we track that this IP has been updated
                domain.LastIp = _lastIp;
                domain.LastUpdated = DateTime.UtcNow;

                RnLogger.Instance.Info(string.Format(
                    "Success :: Updated IP address for '{0}' ({1})",
                    domain.HostName, serverResponse));

                return true;
            }
            catch (Exception ex)
            {
                RnLogger.Instance.Error(string.Format(
                    "Error while trying to update IP Address for '{0}'",
                    domain.HostName),
                    ex);
            }

            return false;
        }


        // http://www.trackip.net/ip?json
        // http://www.trackip.net/ip
        // http://checkip.dyndns.org/
        // http://www.telize.com/ip

        private void RefreshIpAddress()
        {
            if (string.IsNullOrWhiteSpace(_lastIp))
            {
                RnLogger.Instance.Debug("Updating my IP as this is the first time I am running");

                _lastIp = GetIpAddress();
                _lastIpCheckTime = DateTime.UtcNow;
                return;
            }

            if ((DateTime.UtcNow - _lastIpCheckTime).TotalMinutes > 5)
            {
                RnLogger.Instance.Debug("It has been more than 5 min since I last updated my IP address, checking again");

                _lastIp = GetIpAddress();
                _lastIpCheckTime = DateTime.UtcNow;
            }
        }

        private static string GetIpAddress()
        {
            var ipAddress = string.Empty;
            const string url = "http://www.trackip.net/ip";

            try
            {
                RnLogger.Instance.Info("Attempting to fetch my IP Address");

                ServicePointManager.ServerCertificateValidationCallback = CustomCertValidation;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.UserAgent = "DDNS-Enterprise-Client-v2.0";
                httpWebRequest.Method = "GET";
                var responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
                ipAddress = new StreamReader(responseStream).ReadToEnd();
                responseStream.Close();

                if (!RegexHelper.IsValidIpAddress(ipAddress))
                {
                    RnLogger.Instance.Warn(string.Format(
                        "This is not a valid IP Address. {0}", ipAddress));
                    ipAddress = string.Empty;
                }
                else
                {
                    RnLogger.Instance.Debug(string.Format("Success, got IP :: {0}", ipAddress));    
                }
            }
            catch (Exception ex)
            {
                RnLogger.Instance.Error("Error while getting IP address", ex);
            }

            return ipAddress;
        }

        private static bool CustomCertValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
