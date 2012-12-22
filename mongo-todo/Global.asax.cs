using Domain.Repository;
using MongoDB.Bson;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace mongo_todo
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication :System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			for (int i = 0; i < 5; i++) {
				DummyGlobal.Instance.BaseUserObjectId.Add(ObjectId.GenerateNewId());
				DummyGlobal.Instance.BaseTaskObjectId.Add(ObjectId.GenerateNewId());
			}

			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AutoMapperConfig.RegisterMaps();

			var formatters = GlobalConfiguration.Configuration.Formatters;
			var xmlFormatter = formatters.XmlFormatter;
			formatters.Remove(xmlFormatter);

			var jsonFormatter = formatters.JsonFormatter;
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			Bootstrapper.Initialise();
		}
	}
}