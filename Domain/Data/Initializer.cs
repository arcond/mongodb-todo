using MongoDB.Driver;

namespace Domain.Data
{
	public class Initializer
	{
		private readonly MongoDatabase _database;
		public Initializer(MongoDatabase database)
		{
			_database = database;
			if (!_database.CollectionExists("users"))
				_database.CreateCollection("users");

			if (!_database.CollectionExists("todos"))
				_database.CreateCollection("todos");
		}
	}
}
