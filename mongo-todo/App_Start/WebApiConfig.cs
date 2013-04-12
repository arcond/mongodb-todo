using System.Web.Http;

namespace mongo_todo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
									   "ActionApi",
									   "api/{controller}/{id}",
									   new {
										   id = RouteParameter.Optional
									   }
				);
			config.Routes.MapHttpRoute(
									   "TaskApi",
									   "api/users/{userId}/{controller}/{id}",
									   new {
										   id = RouteParameter.Optional
									   }
				);
		}
	}
}