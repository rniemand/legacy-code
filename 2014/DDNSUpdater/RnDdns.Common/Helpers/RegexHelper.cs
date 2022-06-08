using System.Text.RegularExpressions;

namespace RnDdns.Common.Helpers
{
    public static class RegexHelper
    {
        private const string RxIp = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

        public static bool IsValidIpAddress(string ip)
        {
            return Regex.IsMatch(ip, RxIp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
    }
}
