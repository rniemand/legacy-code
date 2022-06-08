using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RnUtils.Logging;
using SnippyWeb.Snippy;

namespace SnippyWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var userName = "richardn";

            dynamic table = SnippyDbHelper.UsersTable();

            table.Insert(new {userName, lastSeen = DateTime.UtcNow});



            return View();
        }

    }
}
