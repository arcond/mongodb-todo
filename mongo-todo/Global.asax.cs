using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json.Serialization;

namespace mongo_todo
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication :System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			Bootstrapper.Initialise();
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			AutoMapperConfig.RegisterMaps();

			var formatters = GlobalConfiguration.Configuration.Formatters;
			var xmlFormatter = formatters.XmlFormatter;
			formatters.Remove(xmlFormatter);
			formatters.Add(xmlFormatter);

			var jsonFormatter = formatters.JsonFormatter;
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}
	}
}