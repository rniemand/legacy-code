using System;
using System.Web.Mvc;
using RnCore.Logging;

namespace Rn.WebApps.RnRss.Web.RnMvc
{
    public static class RnMvcUtils
    {
        public static MvcHtmlString IfActivePage(this ViewContext v, string controller, string action = "Index",
                                                 string active = "active", string notActive = "")
        {
            try
            {
                if (controller.ToLower() == v.RouteData.GetRequiredString("controller").ToLower() &&
                    action.ToLower() == v.RouteData.GetRequiredString("action").ToLower())
                    return new MvcHtmlString(active);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return new MvcHtmlString(notActive);
        }

    }
}