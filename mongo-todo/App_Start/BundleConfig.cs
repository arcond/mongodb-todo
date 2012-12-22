using System.Web.Optimization;

namespace mongo_todo
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/lib")
				.Include("~/scripts/lib/jquery-{version}.js")
				.Include("~/scripts/lib/handlebars.js")
				.Include("~/scripts/lib/bootstrap.js")
				.Include("~/scripts/lib/underscore.js")
				.Include("~/scripts/lib/backbone.js")
				.Include("~/scripts/extensions.js")
				);
		}
	}
}