using System;

namespace NzbScraper.Common.Models
{
    public class NzbInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public string DownloadUrl { get; set; }
        public string PostId { get; set; }
        public string Group { get; set; }
        public string Poster { get; set; }
        public string MoreInfoUrl { get; set; }
        public string SizeString { get; set; }
        public long Size { get; set; }
        public string FileList { get; set; }
        public string AvailableParts { get; set; }
        public int PercentageAvailable { get; set; }
    }
}
