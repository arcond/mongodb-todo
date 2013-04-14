using Domain;
using MongoDB.Bson;
using System.Collections.Generic;

namespace MongoData.Factory
{
	public class MongoDbUserFactory :IUserFactory
	{
		public IUser CreateUser(string name)
		{
			return new MongoDbUser {
				Id = new ObjectId(),
				Name = name,
				Todos = new List<ObjectId>()
			};
		}
	}
}