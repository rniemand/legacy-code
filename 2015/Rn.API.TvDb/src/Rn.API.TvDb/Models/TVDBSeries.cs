using System;
using System.Diagnostics;

namespace Rn.API.TVDB.Models
{
    [DebuggerDisplay("{Name} ({Network})")]
    public class TVDBSeries
    {
        public int SeriesId { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string BannerUrl { get; set; }
        public string Overview { get; set; }
        public DateTime? FirstAired { get; set; }
        public string Network { get; set; }
        public string IMDBId { get; set; }
        public string Zap2itId { get; set; }
        public int Id { get; set; }
    }
}
