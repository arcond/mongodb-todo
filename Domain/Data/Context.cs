using MongoDB.Driver;

namespace Domain.Data
{
	public class Context :IContext
	{
		public Context()
		{
			var client = new MongoClient("");
			var server = client.GetServer();
			Database = server.GetDatabase("");
		}

		public MongoDatabase Database { get; private set; }
	}
}
