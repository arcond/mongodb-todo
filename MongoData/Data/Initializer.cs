using MongoDB.Driver;

namespace MongoData.Data
{
	public class Initializer
	{
		private readonly MongoDatabase _database;

		public Initializer(MongoDatabase database)
		{
			_database = database;
			
			if (!_database.CollectionExists(Collections.USERS))
				_database.CreateCollection(Collections.USERS);

			if (!_database.CollectionExists(Collections.TODOS))
				_database.CreateCollection(Collections.TODOS);
		}
	}
}