using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NzbScraper.Common.Helpers;
using NzbScraper.Services.Extensions;

namespace NzbScraper.Services.Parsers
{
    public static class BinSearchParser
    {
        public static string GetGroupName(string description)
        {
            const string rxp = ">Group:.*?<td>([^<]+)";

            if (!description.IsRxMatch(rxp))
            {
                return string.Empty;
            }

            return description.GetRxMatch(rxp).Groups[1].Value;
        }

        public static string GetPoster(string description)
        {
            const string rxp = ">Poster:.*?<td>([^<]+)";

            if (!description.IsRxMatch(rxp))
            {
                return string.Empty;
            }

            return StringHelper.UrlDecode(description.GetRxMatch(rxp).Groups[1].Value);
        }

        public static string GetMoreInfoLink(string description)
        {
            const string rxp = "<a href=\"([^\"]+)\">collection<\\/a>";

            if (!description.IsRxMatch(rxp))
            {
                return string.Empty;
            }

            return StringHelper.UrlDecode(description.GetRxMatch(rxp).Groups[1].Value);
        }

        public static string GetPostSize(string description)
        {
            const string rxp = "size: ([\\d\\.]+&nbsp;\\w+),";

            if (!description.IsRxMatch(rxp))
            {
                return string.Empty;
            }

            return StringHelper.UrlDecode(description.GetRxMatch(rxp).Groups[1].Value);
        }

        public static string GetPostId(string link)
        {
            const string rxp = "nzb&(\\d+)=1";

            if (!link.IsRxMatch(rxp))
            {
                return string.Empty;
            }

            return link.GetRxMatch(rxp).Groups[1].Value;
        }

        public static string GetAvailableParts(string description)
        {
            const string rxp = "parts available:.*?(\\d+ / \\d+)";

            if (!description.IsRxMatch(rxp))
            {
                return string.Empty;
            }

            return StringHelper.UrlDecode(description.GetRxMatch(rxp).Groups[1].Value);
        }

        public static string GetFileList(string description)
        {
            const string rxp = "<br>- (\\d+ .*? file(s|))";

            if (!description.IsRxMatch(rxp))
            {
                return string.Empty;
            }

            var matches = description.GetRxMatchs(rxp);

            var fileList = matches
                .Cast<Match>()
                .Select(match => match.Groups[1].Value)
                .ToList();

            return string.Join("|", fileList);
        }

        public static long GetSizeAsLong(string size)
        {
            if (string.IsNullOrWhiteSpace(size))
            {
                return 0;
            }

            var parts = size.Split(' ');
            var multiplier = 1;
            var sizeMeasure = parts[1].Trim().ToLower();
            var sizeValue = double.Parse(parts[0]);

            switch (sizeMeasure)
            {
                case "b":
                case "bytes":
                    multiplier = 1;
                    break;
                case "kb":
                    multiplier = 1024;
                    break;
                case "mb":
                    multiplier = 1024 * 1024;
                    break;
                case "gb":
                    multiplier = 1024 * 1024 * 1024;
                    break;
                default:
                    throw new NotImplementedException(string.Format(
                        "Unknown size multiplier '{0}'", sizeMeasure));
            }

            return (long) Math.Floor(sizeValue*multiplier);
        }

        public static int GetAvailablePercentage(string availableParts)
        {
            if (string.IsNullOrWhiteSpace(availableParts))
            {
                return 0;
            }

            var parts = availableParts.Split('/');
            var totalParts = double.Parse(parts[1].Trim());
            var actualParts = double.Parse(parts[0].Trim());

            return (int) Math.Floor((actualParts/totalParts)*100);
        }
    }
}
