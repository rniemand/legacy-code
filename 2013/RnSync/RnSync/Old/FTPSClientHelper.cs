using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using AlexPilotti.FTPS.Client;
using RnUtils.Logging;

namespace RnSync.Old
{
    public static class FtpsClientHelper
    {

        public static bool EnsureClientConnected(this FTPSClient client, FtpSyncClientConfig config)
        {
            if (client == null) return false;
            if (!String.IsNullOrEmpty(client.WelcomeMessage))
                return true;

            try
            {
                RnLogger.Debug("Creating FTP connection to '{0}'", config.FtpHost);
                var esslMode = config.UseClearText ? ESSLSupportMode.ClearText : ESSLSupportMode.All;
                var cred = new NetworkCredential(config.FtpUserName, config.FtpPassword);

                if (config.AcceptAllCerts)
                {
                    RnLogger.Debug("Accepting all remote certificates...");
                    client.Connect(config.FtpHost, cred, esslMode, ValidateTestServerCertificate);
                }
                else
                {
                    client.Connect(config.FtpHost, cred, esslMode);
                }

                RnLogger.Debug("We are now connected to '{0}'", config.FtpHost);
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

        public static bool ValidateTestServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            RnLogger.Info("Accepting client cert...");
            return true;
        }

        public static string AppendSlash(this string s, string slash = @"\")
        {
            if (s.Substring(s.Length - 1, 1) != slash)
                s = String.Format("{0}{1}", s, slash);
            return s;
        }

        public static string StripSlash(this string s, string slash = @"\")
        {
            return s.Substring(s.Length - 1, 1) == slash ? s.Substring(0, s.Length - 1) : s;
        }

        public static bool CanRefreshInfo(this FtpSyncClientConfig cfg, DateTime lastUpdated)
        {
            try
            {
                var timeSpan = DateTime.Now - lastUpdated;
                return cfg.RefreshInfoThresholdMin >= timeSpan.TotalMinutes;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return false;
        }

    }
}
