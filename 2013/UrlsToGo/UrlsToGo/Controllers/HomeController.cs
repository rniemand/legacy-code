using System;
using System.Threading;
using System.Web.Mvc;
using UrlsToGo.Models;
using UrlsToGo.RnUtils;

namespace UrlsToGo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            RnPageScripts.Instance.AddScript("/Scripts/pages/jquery.qrcode.min.js", "js");

            return View();
        }

        public ActionResult FollowLink(string id)
        {
            ViewBag.LinkId = id;

            RnPageScripts.Instance.AddScript("/Scripts/pages/jquery.qrcode.min.js", "js");

            var url = new ShortUrl(id);
            ViewBag.url = url;

            // https://github.com/jeromeetienne/jquery-qrcode

            /*
            Renderer renderer = new Renderer(11, System.Drawing.Brushes.Black, System.Drawing.Brushes.White);
            renderer.CreateImageFile(qrCode.Matrix, @"C:\Users\user\Desktop\wpf\WpfApplication2\WpfApplication2\Images\QR.png\Images\QR.png",
                ImageFormat.Png);
            */
            

            if (!String.IsNullOrEmpty(url.UrlFull))
            {
                url.UpdateHitCounter();
                return Redirect(url.UrlFull);
            }

            // todo: error page
            return View();
        }

        [HttpPost]
        public ActionResult ShortenUrl(FormCollection post)
        {
            var url = post.GetKeyValue("url");
            Thread.Sleep(500);
            return Json(String.IsNullOrEmpty(url) ? new ShortUrl() : new ShortUrl(url), JsonRequestBehavior.AllowGet);
        }

    }
}
