using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AMS.Profile;
using System.Windows.Forms;

namespace AdslSwitcher2.Classes
{
    class AccountSwitcherHelper
    {
        private Logger _logger;
        private Xml _accountXML;
        public int EncodingPasses = 5;

        public string XMLConfigFile { get; private set; }
        public string WorkingDirectory { get; private set; }
        public string ConfigFileFullPath {get; private set;}
        public bool ConfigFileExists { get; private set; }

        public AccountSwitcherHelper(Logger logger)
        {
            _logger = logger;
            ConfigFileExists = false;

            logger.WriteEntry("Creating AccountSwitcher Class", LogLevel.Debug);

            WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
            XMLConfigFile = "ADSL_Switcher_Config.xml";
            ConfigFileFullPath = WorkingDirectory + XMLConfigFile;

            CheckConfigFile();

            if(ConfigFileExists)
                _accountXML = new Xml(ConfigFileFullPath);
        }

        public void CheckConfigFile()
        {
            _logger.WriteEntry("Looking for config XML", LogLevel.Debug);

            if (!File.Exists(ConfigFileFullPath))
            {
                _logger.WriteEntry("Config XML not found", LogLevel.Warning);
                string lf = Environment.NewLine;
                
                // Create the config file data
                StringBuilder ConfigFileData = new StringBuilder();
                ConfigFileData.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + lf);
                ConfigFileData.Append("<profile>" + lf);
                ConfigFileData.Append("<section name=\"accounts\">" + lf);
                ConfigFileData.Append("</section>" + lf);
                ConfigFileData.Append("</profile>");

                // try to create the file
                _logger.WriteEntry("Attempting to create config XML", LogLevel.Info);

                try
                {
                    File.WriteAllText(ConfigFileFullPath, ConfigFileData.ToString());
                }
                catch (Exception e)
                {
                    _logger.WriteEntry("Error creating config XML", LogLevel.Error);
                    var errorMsg = new StringBuilder();
                    errorMsg.Append("There was an error creating the " + XMLConfigFile + " in the following directory: ");
                    errorMsg.Append(WorkingDirectory + "." + Environment.NewLine + Environment.NewLine + "Error Type: ");
                    errorMsg.Append(e.GetType().ToString() + Environment.NewLine + "Error Msg: " + e.Message);
                    _logger.WriteToLog(errorMsg.ToString(), LogLevel.Error);
                    return;
                }
            }

            ConfigFileExists = true;
            _logger.WriteEntry("Config XML created", LogLevel.Info);
        }

        #region Working With Account XML
        public void UpdateADSLAccount(ADSLAccountStruct info)
        {
            string accountInfo = info.UserName + "|" + info.UserPass + "|" + info.DefaultAccount.ToString();
            string adslAccountInfo = AdslMisc.ToBase64(accountInfo, EncodingPasses);
            _accountXML.SetValue("accounts", info.DisplayName, adslAccountInfo);
        }

        public void SaveADSLAccount(ADSLAccountStruct info)
        {
            // Decide on the action to take
            if (!_accountXML.HasEntry("accounts", info.DisplayName))
            {
                string accountInfo = info.UserName + "|" + info.UserPass + "|" + info.DefaultAccount.ToString();
                string adslAccountInfo = AdslMisc.ToBase64(accountInfo, EncodingPasses);
                _accountXML.SetValue("accounts", info.DisplayName.ToString(), adslAccountInfo);
            }
            else
            {
                UpdateADSLAccount(info);
            }
        }

        public bool HasSavedADSLAccounts()
        {
            if (_accountXML.GetEntryNames("accounts").Length > 0) return true;
            return false;
        }

        public string[] GetADSLAccounts()
        {
            return _accountXML.GetEntryNames("accounts");
        }

        public void SetDefaultADSLAccount(string accountName)
        {
            if (_accountXML.HasEntry("accounts", accountName))
            {
                // Set all account defaults to FALSE
                if (_accountXML.GetEntryNames("accounts").Length > 1)
                {
                    foreach (string account in _accountXML.GetEntryNames("accounts"))
                    {
                        string tmpAccountInfo = _accountXML.GetValue("accounts", account).ToString();
                        ADSLAccountStruct tmpAccountObj = AdslMisc.DecodeADSLAccountData(tmpAccountInfo, account, EncodingPasses);
                        tmpAccountObj.DefaultAccount = false;
                        UpdateADSLAccount(tmpAccountObj);
                    }
                }

                // Get information for the current account
                string accountInfo = _accountXML.GetValue("accounts", accountName).ToString();

                ADSLAccountStruct objAccount = AdslMisc.DecodeADSLAccountData(accountInfo, accountName, EncodingPasses);
                objAccount.DefaultAccount = true;
                SaveADSLAccount(objAccount);

                _logger.WriteEntry("Default Account Set", LogLevel.Info);
            }
        }

        public string GetADSLAccountData(string accountName)
        {
            if (_accountXML.HasEntry("accounts", accountName))
            {
                return _accountXML.GetValue("accounts", accountName).ToString();
            }

            return string.Empty;
        }

        public bool ADSLAccountExists(string accountName)
        {
            return _accountXML.HasEntry("accounts", accountName);
        }

        public ADSLAccountStruct GetADSLAccount(string accountName)
        {
            string accountInfo = GetADSLAccountData(accountName);
            return AdslMisc.DecodeADSLAccountData(accountInfo, accountName, EncodingPasses);
        }

        public void DeleteADSLAccount(string accountName)
        {
            if (ADSLAccountExists(accountName))
                _accountXML.RemoveEntry("accounts", accountName);
        }

        public void RemoveConfigFile()
        {
            File.Delete(ConfigFileFullPath);
        }
        #endregion

        #region Router Login Credentials
        public bool HasRouterUserName()
        {
            if (_accountXML.HasEntry("RouterLogin", "UserName"))
                return true;
            return false;
        }

        public string GetRouterUserName()
        {
            return AdslMisc.FromBase64(_accountXML.GetValue("RouterLogin", "UserName").ToString(), EncodingPasses);
        }

        public bool HasRouterUserPass()
        {
            if (_accountXML.HasEntry("RouterLogin", "UserPass"))
                return true;
            return false;
        }

        public string GetRouterUserPass()
        {
            return AdslMisc.FromBase64(_accountXML.GetValue("RouterLogin", "UserPass").ToString(), EncodingPasses);
        }

        public void SaveRouterUserName(string userName)
        {
            _accountXML.SetValue("RouterLogin", "UserName", AdslMisc.ToBase64(userName, EncodingPasses));
        }

        public void SaveRouterUserPass(string password)
        {
            _accountXML.SetValue("RouterLogin", "UserPass", AdslMisc.ToBase64(password, EncodingPasses));
        }
        #endregion

        #region Working with IP Addresses
        public void SaveIPAddress(string ipAddress)
        {
            _accountXML.SetValue("RouterLogin", "IPAddress", ipAddress);
        }

        public string GetIPAddress()
        {
            return _accountXML.GetValue("RouterLogin", "IPAddress").ToString();
        }

        public bool HasIPAddress()
        {
            if (_accountXML.HasEntry("RouterLogin", "IPAddress"))
                return true;
            return false;
        }
        #endregion

    }
}
