using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace mongo_todo
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication :HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AutoMapperConfig.RegisterMaps();

			MediaTypeFormatterCollection formatters =
				GlobalConfiguration.Configuration.Formatters;
			XmlMediaTypeFormatter xmlFormatter = formatters.XmlFormatter;
			formatters.Remove(xmlFormatter);

			JsonMediaTypeFormatter jsonFormatter = formatters.JsonFormatter;
			jsonFormatter.SerializerSettings.ContractResolver =
				new CamelCasePropertyNamesContractResolver();

			Bootstrapper.Initialise();
		}
	}
}