using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NzbScraper.Common.Models;
using NzbScraper.Services.Parsers;

namespace NzbScraper.Services
{
    public class BinSearchRssProcessor : RssFileProcessorBase
    {
        public BinSearchRssProcessor(string file)
            : base(file)
        {
            // nothing to see here
        }

        public List<NzbInfo> GetItems()
        {
            return GetItemNodes()
                .Cast<XmlNode>()
                .Select(ProcessRssItem)
                .ToList();
        }

        private static NzbInfo ProcessRssItem(XmlNode item)
        {
            var title = item.SelectSingleNode("title").InnerText;
            var description = item.SelectSingleNode("description").InnerText;
            var pubDate = DateTime.Parse(item.SelectSingleNode("pubDate").InnerText);
            var link = item.SelectSingleNode("link").InnerText;

            var postId = BinSearchParser.GetPostId(link);
            var group = BinSearchParser.GetGroupName(description);
            var poster = BinSearchParser.GetPoster(description);
            var moreInfo = BinSearchParser.GetMoreInfoLink(description);
            var size = BinSearchParser.GetPostSize(description);
            var availableParts = BinSearchParser.GetAvailableParts(description);
            var fileList = BinSearchParser.GetFileList(description);
            var fileSize = BinSearchParser.GetSizeAsLong(BinSearchParser.GetPostSize(description));
            var percentageAvailable = BinSearchParser.GetAvailablePercentage(availableParts);

            return new NzbInfo
            {
                Title = title,
                Description = description,
                PublishedDate = pubDate,
                FileList = fileList,
                DownloadUrl = link,
                Group = group,
                MoreInfoUrl = moreInfo,
                PostId = postId,
                Poster = poster,
                Size = fileSize,
                SizeString = size,
                AvailableParts = availableParts,
                PercentageAvailable = percentageAvailable
            };
        }
    }
}
