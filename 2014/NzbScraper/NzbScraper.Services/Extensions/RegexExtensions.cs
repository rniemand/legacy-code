using System.Text.RegularExpressions;

namespace NzbScraper.Services.Extensions
{
    public static class RegexExtensions
    {
        public static bool IsRxMatch(this string input, string rxp)
        {
            return Regex.IsMatch(input, rxp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static Match GetRxMatch(this string input, string rxp)
        {
            return Regex.Match(input, rxp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static MatchCollection GetRxMatchs(this string input, string rxp)
        {
            return Regex.Matches(input, rxp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
    }
}
