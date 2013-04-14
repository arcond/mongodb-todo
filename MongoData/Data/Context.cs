using Domain;
using MongoDB.Driver;
using System.Configuration;

namespace MongoData.Data
{
	public class Context :IContext
	{
		public Context()
		{
			var client =
				new MongoClient(
					ConfigurationManager.ConnectionStrings["mongodb"].ConnectionString);
			var server = client.GetServer();
			Database = server.GetDatabase(Collections.DATABASE);
			var initializer = new Initializer(Database);
		}

		public MongoDatabase Database { get; private set; }
	}
}