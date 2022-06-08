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
    public partial class AccountAddForm : Form
    {
        private Logger _logger;
        private AccountSwitcherHelper _accountSwitcher;

        public bool AddMode { get; set; }
        public ADSLAccountStruct ADSLAccountInfo { get; set; }

        public AccountAddForm()
        {
            InitializeComponent();

            _logger = new Logger(toolStripStatusLabel1);
            _accountSwitcher = new AccountSwitcherHelper(_logger);

            _logger.WriteEntry("Window Opened", LogLevel.Info);
        }

        #region Form Value Getters
        private string GetAccountDisplayName()
        {
            return textBoxDisplayName.Text;
        }

        private string GetAccountUserName()
        {
            return textBoxUserName.Text;
        }

        private string GetAccountPassword()
        {
            return textBoxUserPass.Text;
        } 
        #endregion

        #region Value Setters
        private void SetDisplayName(string displayName)
        {
            textBoxDisplayName.Text = displayName;
        }

        private void SetUserName(string userName)
        {
            textBoxUserName.Text = userName;
        }

        private void SetPassWord(string password)
        {
            textBoxUserPass.Text = password;
        } 
        #endregion

        #region Form Actions
        private void AccountAddForm_Load(object sender, EventArgs e)
        {
            if (AddMode)
            {
                buttonAction.Text = "Add";
            }
            else
            {
                buttonAction.Text = "Save";
                SetEditFormValues();
            }
        }

        private void SetEditFormValues()
        {
            SetDisplayName(ADSLAccountInfo.DisplayName);
            SetUserName(ADSLAccountInfo.UserName);
            SetPassWord(ADSLAccountInfo.UserPass);
        }

        private void buttonAction_Click(object sender, EventArgs e)
        {
            ADSLAccountStruct accountInfo = new ADSLAccountStruct();
            accountInfo.DisplayName = GetAccountDisplayName();
            accountInfo.UserName = GetAccountUserName();
            accountInfo.UserPass = GetAccountPassword();

            // Decide what to do
            if (AddMode)
            {
                _accountSwitcher.SaveADSLAccount(accountInfo);
                ResetForm();
                _logger.WriteEntry("Account Added", LogLevel.Info);
            }
            else
            {
                // Save the current account
                accountInfo.DefaultAccount = ADSLAccountInfo.DefaultAccount;
                _accountSwitcher.UpdateADSLAccount(accountInfo);
                _logger.WriteEntry("Account Updated", LogLevel.Info);
            }
        }

        private void ResetForm()
        {
            SetDisplayName("");
            SetUserName("");
            SetPassWord("");
            textBoxDisplayName.Focus();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        } 
        #endregion


    }
}
