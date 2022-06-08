using System.Web.Mvc;
using RnCore.Mvc;
using RnCore.Mvc.Permissions;

namespace Rn.WebApps.RnRss.Web.Controllers
{
    public class HomeController : RnMvcControllerBase
    {

        public ActionResult Index()
        {
            //FormsAuthentication.SetAuthCookie("richard", true); 
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Members()
        {
            return View();
        }

    }
}
