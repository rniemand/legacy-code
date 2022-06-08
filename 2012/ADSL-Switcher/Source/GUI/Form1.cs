using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdslSwitcher2.Classes;

namespace AdslSwitcher2
{
    public partial class Form1 : Form
    {
        Logger _logger;
        AccountSwitcherHelper _accountSwitcherHelper;

        private string _adslAccount;
        private string _routerLoginName;
        private string _routerLoginPass;
        private string _ipAddress;

        #region Form Population
        public Form1()
        {
            InitializeComponent();

            _logger = new Logger(toolStripStatusLabel);
            _accountSwitcherHelper = new AccountSwitcherHelper(_logger);

            LoadFormDefaultValues();
        }

        private void LoadFormDefaultValues()
        {
            LoadADSLAccounts();
            SetRouterLoginName();
            SetRouterLoginPass();
            SetIPAddress();
        } 
        #endregion

        #region Working With Login Credentials
        private void SetRouterLoginName()
        {
            if (_accountSwitcherHelper.HasRouterUserName())
            {
                textBoxUserName.Text = _accountSwitcherHelper.GetRouterUserName();
                _routerLoginName = textBoxUserName.Text;
            }
        }

        private void SetRouterLoginPass()
        {
            if (_accountSwitcherHelper.HasRouterUserPass())
            {
                textBoxUserPass.Text = _accountSwitcherHelper.GetRouterUserPass();
                _routerLoginPass = textBoxUserPass.Text;
            }
        }

        private string GetRouterLoginName()
        {
            _routerLoginName = textBoxUserName.Text;
            return _routerLoginName;
        }

        private string GetRouterLoginPass()
        {
            _routerLoginPass = textBoxUserPass.Text;
            return _routerLoginPass;
        }

        private void SaveRouterLoginDetails()
        {
            _accountSwitcherHelper.SaveRouterUserName(GetRouterLoginName());
            _accountSwitcherHelper.SaveRouterUserPass(GetRouterLoginPass());
        }
        #endregion

        #region ADSL Accounts
        private void LoadADSLAccounts()
        {
            if (_accountSwitcherHelper.HasSavedADSLAccounts())
            {
                string[] ADSLAccounts = _accountSwitcherHelper.GetADSLAccounts();
                foreach (string account in ADSLAccounts)
                    AddADSLAccount(account, false);
            }

            //comboBoxAccount
        }

        private void AddADSLAccount(string accountName, bool selected)
        {
            string accountData = _accountSwitcherHelper.GetADSLAccountData(accountName);
            ADSLAccountStruct objAccount = new ADSLAccountStruct();
            
            if( accountData != string.Empty)
                objAccount = AdslMisc.DecodeADSLAccountData(accountData, accountName, _accountSwitcherHelper.EncodingPasses);

            if (!comboBoxAccount.Items.Contains(accountName))
            {
                comboBoxAccount.Items.Add(accountName);
            }

            if (selected || objAccount.DefaultAccount)
            {
                comboBoxAccount.SelectedIndex = comboBoxAccount.Items.IndexOf(accountName);
                comboBoxAccount.Text = accountName;
                _adslAccount = accountName;
            }
        }

        private string GetSelectedADSLAccount()
        {
            _adslAccount = comboBoxAccount.Text;
            return _adslAccount;
        }
        #endregion

        #region Get Form Values
        public string GetSelectedAccount()
        {
            if (comboBoxAccount.SelectedIndex > -1)
            {
                string selectedValue = comboBoxAccount.Items[comboBoxAccount.SelectedIndex].ToString();
                _adslAccount = selectedValue;
                return selectedValue;
            }

            return string.Empty;
        } 
        #endregion

        #region Button Clicks / Events
        
        private void buttonAccountAdd_Click(object sender, EventArgs e)
        {
            AccountAddForm form = new AccountAddForm();
            form.AddMode = true;
            form.Show();
        }

        private void setAccountAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _accountSwitcherHelper.SetDefaultADSLAccount(GetSelectedADSLAccount());
        }

        private void comboBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonAccountEdit_Click(object sender, EventArgs e)
        {
            string AccountName = GetSelectedAccount();

            if (_accountSwitcherHelper.ADSLAccountExists(AccountName))
            {
                AccountAddForm form = new AccountAddForm();
                form.AddMode = false;
                form.ADSLAccountInfo = _accountSwitcherHelper.GetADSLAccount(AccountName);
                form.Show();
            }
            else
            {
                MessageBox.Show("You need to select an account to edit!", "Select and Account");
            }

        }

        private void buttonAccountDelete_Click(object sender, EventArgs e)
        {
            string accountName = GetSelectedAccount();
            if (accountName == string.Empty)
            {
                MessageBox.Show("You need to select an account to delete first");
                return;
            }

            // Confirm
            string confirmMsg = "Are you sure you want to delete: " + accountName;
            if (MessageBox.Show(confirmMsg, "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _accountSwitcherHelper.DeleteADSLAccount(accountName);

                if (comboBoxAccount.Items.Contains(accountName))
                {
                    comboBoxAccount.SelectedIndex = -1;
                    comboBoxAccount.Items.Remove(accountName);
                    if (comboBoxAccount.Items.Count > 0)
                        comboBoxAccount.SelectedIndex = 0;
                }

                _logger.WriteEntry("Removed " + accountName, LogLevel.Info);
            }
            
        }
        #endregion

        #region Working With the IP Address
        private void textBoxIP1_TextChanged(object sender, EventArgs e)
        {
            if (textBoxIP1.Text.Length == 3) textBoxIP2.Focus();
            textBoxIP1.Text = AdslMisc.GetNumbersOnly(textBoxIP1.Text);
        }

        private void textBoxIP2_TextChanged(object sender, EventArgs e)
        {
            if (textBoxIP2.Text.Length == 3) textBoxIP3.Focus();
            textBoxIP2.Text = AdslMisc.GetNumbersOnly(textBoxIP2.Text);
        }

        private void textBoxIP3_TextChanged(object sender, EventArgs e)
        {
            if (textBoxIP3.Text.Length == 3) textBoxIP4.Focus();
            textBoxIP3.Text = AdslMisc.GetNumbersOnly(textBoxIP3.Text);
        }

        private void textBoxIP4_TextChanged(object sender, EventArgs e)
        {
            if (textBoxIP4.Text.Length == 3) buttonConnectionTest.Focus();
            textBoxIP4.Text = AdslMisc.GetNumbersOnly(textBoxIP4.Text);
        }

        private void textBoxIP1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 190 || e.KeyValue == 110)
            {
                textBoxIP1.Text = textBoxIP1.Text.Substring(0, textBoxIP1.Text.Length);
                textBoxIP2.Focus();
                return;
            }
        }

        private void textBoxIP2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 190 || e.KeyValue == 110)
            {
                textBoxIP2.Text = textBoxIP2.Text.Substring(0, textBoxIP2.Text.Length);
                textBoxIP3.Focus();
                return;
            }
        }

        private void textBoxIP3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 190 || e.KeyValue == 110)
            {
                textBoxIP3.Text = textBoxIP3.Text.Substring(0, textBoxIP3.Text.Length);
                textBoxIP4.Focus();
                return;
            }
        }

        private void textBoxIP4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 190 || e.KeyValue == 110)
            {
                textBoxIP4.Text = textBoxIP4.Text.Substring(0, textBoxIP4.Text.Length);
                buttonConnectionTest.Focus();
            }
        }

        private string GetCurrentIPAddress()
        {
            StringBuilder ipAddress = new StringBuilder();
            ipAddress.Append(textBoxIP1.Text + ".");
            ipAddress.Append(textBoxIP2.Text + ".");
            ipAddress.Append(textBoxIP3.Text + ".");
            ipAddress.Append(textBoxIP4.Text);
            _ipAddress = ipAddress.ToString();
            return _ipAddress;
        }

        private void SaveCurrentIPAddress()
        {
            _accountSwitcherHelper.SaveIPAddress(GetCurrentIPAddress());
        }

        private void SetIPAddress()
        {
            if (_accountSwitcherHelper.HasIPAddress())
            {
                string[] ipAddressParts = _accountSwitcherHelper.GetIPAddress().Split('.');
                textBoxIP1.Text = ipAddressParts[0];
                textBoxIP2.Text = ipAddressParts[1];
                textBoxIP3.Text = ipAddressParts[2];
                textBoxIP4.Text = ipAddressParts[3];
            }
        }
        #endregion

        private ADSLRouterStruct GetRouterInfoObject()
        {
            ADSLRouterStruct objRouter = new ADSLRouterStruct();
            objRouter.RouterIP = GetCurrentIPAddress();
            objRouter.RouterLoginPass = GetRouterLoginPass();
            objRouter.RouterLoginUser = GetRouterLoginName();
            return objRouter;
        }

        private bool CheckForNeededValues()
        {
            if (comboBoxAccount.Text == string.Empty || comboBoxAccount.Text == "" || comboBoxAccount.Text.Length < 1)
            {
                _logger.WriteEntry("YOU NEED AN ACCOUNT", LogLevel.Error);
                comboBoxAccount.Focus();
                return false;
            }

            if (comboBoxAccount.SelectedIndex == -1)
            {
                _logger.WriteEntry("PLEASE ADD AN ACCOUNT WITH THE + BUTTON", LogLevel.Error);
                comboBoxAccount.Focus();
                return false;
            }

            if (GetCurrentIPAddress().Length < 5)
            {
                _logger.WriteEntry("YOU NEED AN IP ADDRESS", LogLevel.Error);
                textBoxIP1.Focus();
                return false;
            }

            if (textBoxUserName.Text == string.Empty || textBoxUserName.Text == "" || textBoxUserName.Text.Length < 1)
            {
                _logger.WriteEntry("YOU NEED A LOGIN NAME", LogLevel.Error);
                textBoxUserName.Focus();
                return false;
            }

            if (textBoxUserPass.Text == string.Empty || textBoxUserPass.Text == "" || textBoxUserPass.Text.Length < 1)
            {
                _logger.WriteEntry("YOU NEED A LOGIN PASS", LogLevel.Error);
                textBoxUserPass.Focus();
                return false;
            }

            



            return true;
        }

        private void buttonApplyChange_Click(object sender, EventArgs e)
        {
            if (!CheckForNeededValues())
            {
                MessageBox.Show("Please add in the required information");
                return;
            }
            SaveRouterLoginDetails();
            SaveCurrentIPAddress();

            ADSLAccountStruct objAccount = _accountSwitcherHelper.GetADSLAccount(GetSelectedADSLAccount());
            ADSLRouterStruct objRouter = GetRouterInfoObject();

            AccountSwitcher switcher = new AccountSwitcher(objAccount, objRouter, _logger);
            switcher.SwitchAccount();
        }

        private void buttonConnectionTest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implimented Yet");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.Show();
        }

        private void forgetDefaultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string erm = "Are you sure you want to forget all your settings? This cannot be undone!";
            
            if (MessageBox.Show(erm, "READ THIS!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Delete Config File
                _accountSwitcherHelper.RemoveConfigFile();
                
                // Clear Form
                comboBoxAccount.Text = "";
                comboBoxAccount.Items.Clear();
                textBoxIP1.Text = "";
                textBoxIP2.Text = "";
                textBoxIP3.Text = "";
                textBoxIP4.Text = "";
                textBoxUserName.Text = "";
                textBoxUserPass.Text = "";

                // Clear Locals
                _adslAccount = "";
                _routerLoginName = "";
                _routerLoginPass = "";
                _ipAddress = "";

                // Recreate Config File
                _accountSwitcherHelper.CheckConfigFile();

                // Log to the user
                _logger.WriteEntry("Cleared config file", LogLevel.Info);
            }
        }

    }
}
