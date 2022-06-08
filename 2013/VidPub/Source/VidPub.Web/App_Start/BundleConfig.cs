using System.Web;
using System.Web.Optimization;

namespace VidPub.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Public/javascripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Public/javascripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Public/javascripts/jquery.unobtrusive*",
                        "~/Public/javascripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Public/styles/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Public/styles/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Public/styles/themes/base/jquery.ui.core.css",
                        "~/Public/styles/themes/base/jquery.ui.resizable.css",
                        "~/Public/styles/themes/base/jquery.ui.selectable.css",
                        "~/Public/styles/themes/base/jquery.ui.accordion.css",
                        "~/Public/styles/themes/base/jquery.ui.autocomplete.css",
                        "~/Public/styles/themes/base/jquery.ui.button.css",
                        "~/Public/styles/themes/base/jquery.ui.dialog.css",
                        "~/Public/styles/themes/base/jquery.ui.slider.css",
                        "~/Public/styles/themes/base/jquery.ui.tabs.css",
                        "~/Public/styles/themes/base/jquery.ui.datepicker.css",
                        "~/Public/styles/themes/base/jquery.ui.progressbar.css",
                        "~/Public/styles/themes/base/jquery.ui.theme.css"));
        }
    }
}