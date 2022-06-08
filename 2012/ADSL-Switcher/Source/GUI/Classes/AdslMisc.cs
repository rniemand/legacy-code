using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdslSwitcher2.Classes
{
    public static class AdslMisc
    {

        public static string ToBase64(string input, int passes)
        {
            string output = input;

            for (int i = 0; i < passes; i++)
            {
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(output);
                output = System.Convert.ToBase64String(toEncodeAsBytes);
            }

            return output;
        }

        public static string FromBase64(string input, int passes)
        {
            string output = input;

            for (int i = 0; i < passes; i++)
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(output);
                output = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            }

            return output;
        }

        public static ADSLAccountStruct DecodeADSLAccountData(string data, string accountName, int encodingPasses)
        {
            string accountInfo = AdslMisc.FromBase64(data, encodingPasses);
            string[] accountParts = accountInfo.Split('|');

            ADSLAccountStruct objAccount = new ADSLAccountStruct();
            objAccount.DisplayName = accountName;
            objAccount.UserName = accountParts[0];
            objAccount.UserPass = accountParts[1];
            objAccount.DefaultAccount = (accountParts[2] == "True" ? true : false);

            return objAccount;
        }

        public static string GetNumbersOnly(string inputString)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in inputString)
            {
                if ((c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

    }
}
