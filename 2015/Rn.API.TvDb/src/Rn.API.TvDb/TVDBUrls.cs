namespace Rn.API.TVDB
{
    public static class TVDBUrls
    {
        public static string TimeUrl
        {
            get { return "http://thetvdb.com/api/Updates.php?type=none"; }
        }

        public static string SeriesSearch
        {
            get { return "<XML_MIRROR>/api/GetSeries.php?seriesname={0}"; }
        }
    }
}
