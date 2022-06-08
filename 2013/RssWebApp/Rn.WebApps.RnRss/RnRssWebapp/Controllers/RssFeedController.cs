using System.Web.Mvc;
using RnCore.Helpers;
using RnCore.Mvc.Helpers;

namespace Rn.WebApps.RnRss.Web.Controllers
{
    public class RssFeedController : Controller
    {
        //
        // GET: /RssFeed/

        [HttpPost]
        public ActionResult GetFeedContents(FormCollection data)
        {
            // feedUrl
            var feedUrl = data.GetKeyValue("feedUrl");
            var feed = new RnCore.Web.Rss.RssFeed(feedUrl);

            return Json(feed);
        }

    }
}
