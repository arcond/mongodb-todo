using System.Web.Optimization;

namespace mongo_todo
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(
				new ScriptBundle("~/bundles/modernizr").Include(
					"~/Scripts/modernizr-*"));

			bundles.Add(
				new ScriptBundle("~/bundles/lib").Include(
					"~/scripts/lib/jquery-{version}.js",
					"~/scripts/lib/handlebars.js",
					"~/scripts/lib/bootstrap.js",
					"~/scripts/lib/underscore.js",
					"~/scripts/lib/backbone.js",
					"~/scripts/extensions.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

			bundles.Add(
				new StyleBundle("~/Content/themes/base/css").Include(
					"~/content/bootstrap.css",
					"~/content/font-awesome.css"));
		}
	}
}