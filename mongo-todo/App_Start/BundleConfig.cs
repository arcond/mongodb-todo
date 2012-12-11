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
				//.Include("~/scripts/lib/require.js")
				.Include("~/scripts/lib/backbone.js")
				);

			bundles.Add(new ScriptBundle("~/bundles/todo")
				.Include("~/scripts/models.js")
				.Include("~/scripts/collection.js")
				.Include("~/scripts/views.js")
				.Include("~/scripts/routers.js")
				.Include("~/scripts/app.js")
				);

			bundles.Add(new ScriptBundle("~/bundles/extension")
				.Include("~/scripts/extensions.js")
				);
		}
	}
}