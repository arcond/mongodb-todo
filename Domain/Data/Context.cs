using MongoDB.Driver;
using System.Configuration;

namespace Domain.Data
{
	public class Context :IContext
	{
		public Context()
		{
			var client = new MongoClient(ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString);
			var server = client.GetServer();
			Database = server.GetDatabase("TodoSample");
			var initializer = new Initializer(Database);
		}

		public MongoDatabase Database { get; private set; }
	}
}
