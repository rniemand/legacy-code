using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdslSwitcher2
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            StringBuilder info = new StringBuilder();
            info.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Calibri;}{\f1\fmodern\fprq1\fcharset0 Consolas;}{\f2\fnil\fcharset0 Calibri;}}" + Environment.NewLine);
            info.Append(@"{\colortbl ;\red0\green128\blue0;\red43\green145\blue175;\red0\green0\blue255;\red163\green21\blue21;}" + Environment.NewLine);
            info.Append(@"{\*\generator Msftedit 5.41.21.2509;}\viewkind4\uc1\pard\sa200\sl276\slmult1\b\f0\fs22 About ADSL Account Switcher\b0\par" + Environment.NewLine);
            info.Append(@"This application was created with the simple goal in mind to switch between ADSL accounts on the Netgear \b DG384\b0  with ease. I am sick of having to log in each time I want to do this. All login data is stored in an XML file under the current users roaming data (should follow you around). The login data is encrypted (well not human readable) so don\rquote t stress about clear text passwords.\par" + Environment.NewLine);
            info.Append(@"To make the switch I am logging into the router with the following code.\par" + Environment.NewLine);
            info.Append(@"\pard\fi720\cf1\f1\fs16 // Get information needed about the router\cf0\par" + Environment.NewLine);
            info.Append(@"\cf2 Netgear_DG384\cf0  RouterSettings = \cf3 new\cf0  \cf2 Netgear_DG384\cf0 ();\par" + Environment.NewLine);
            info.Append(@"RouterSettings.SetLoginDetails(AccountInfo.UserName, AccountInfo.UserPass);\par" + Environment.NewLine);
            info.Append(@"PostData = RouterSettings.ToString();\par" + Environment.NewLine);
            info.Append(@"PostURL = RouterSettings.GetSubmitUrl(RouterInfo.RouterIP);\par" + Environment.NewLine);
            info.Append(@"\cf3 byte\cf0 [] buffer = \cf2 Encoding\cf0 .ASCII.GetBytes(PostData);\par" + Environment.NewLine);
            info.Append(@"\par" + Environment.NewLine);
            info.Append(@"\cf1 //Connect to the router, send login details, prep for post\cf0\par" + Environment.NewLine);
            info.Append(@"\cf2 HttpWebRequest\cf0  WebReq = (\cf2 HttpWebRequest\cf0 )\cf2 WebRequest\cf0 .Create(PostURL);\par" + Environment.NewLine);
            info.Append(@"WebReq.Credentials = \cf3 new\cf0  \cf2 NetworkCredential\cf0 (RouterInfo.RouterLoginUser, \par" + Environment.NewLine);
            info.Append(@"RouterInfo.RouterLoginPass);\par" + Environment.NewLine);
            info.Append(@"WebReq.Method = \cf4 ""POST""\cf0 ;\par" + Environment.NewLine);
            info.Append(@"WebReq.ContentType = \cf4 ""application/x-www-form-urlencoded""\cf0 ;\par" + Environment.NewLine);
            info.Append(@"WebReq.ContentLength = buffer.Length;\cf1\par" + Environment.NewLine);
            info.Append(@"\cf0\par" + Environment.NewLine);
            info.Append(@"...\par" + Environment.NewLine);
            info.Append(@"\pard\sa200\sl276\slmult1\f0\fs22\line\pard If you would like the source feel free to drop me a mail (niemand.richard@gmail.com).\par" + Environment.NewLine);
            info.Append(@"\pard\sa200\sl276\slmult1\lang9\f2\par" + Environment.NewLine);

            richTextBox1.Rtf = info.ToString();
        }
    }
}
