namespace NzbScraper.Common.Helpers
{
    public static class StringHelper
    {
        public static string UrlDecode(string input)
        {
            return input
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&")
                .Replace("%28", "(")
                .Replace("%29", ")")
                .Replace("%3C", "<")
                .Replace("%3E", ">")
                .Replace("%40", "@")
                .Replace("&nbsp;", " ");
        }
    }
}
