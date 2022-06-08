using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Rn.Helpers
{
    public static class RxHelper
    {
        public static int ExtractNumbers(string s)
        {
            var str = Regex.Replace(s.Trim(), "[a-zA-Z]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return str.ToInt();
        }

        public static string ExtractLetters(string s)
        {
            return Regex.Replace(s.Trim(), "[0-9]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline).Trim();
        }
    }
}
