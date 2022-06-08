using System;
using System.Windows.Forms;
using RnDdns.Common.Helpers;

namespace RnDdns.Tools.Forms
{
    public partial class PasswordHasher : Form
    {
        public PasswordHasher()
        {
            InitializeComponent();
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bEncrypt_Click(object sender, EventArgs e)
        {
            var input = tbInput.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                tbOutput.Text = "";
                tbDecrypted.Text = "";
                return;
            }

            tbOutput.Text = EncryptionHelper.Base64Encode(input);
            tbDecrypted.Text = EncryptionHelper.Base64Decode(tbOutput.Text);
        }
    }
}
