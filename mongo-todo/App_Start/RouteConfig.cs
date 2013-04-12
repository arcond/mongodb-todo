using System.Web.Mvc;
using System.Web.Routing;

namespace mongo_todo
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute(
				"{*favicon}",
				new {
					favicon = @"(.*/)?favicon.ico(/.*)?"
				});

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new {
					controller = "Home",
					action = "Index",
					id = UrlParameter.Optional
				}
				);
		}
	}
}