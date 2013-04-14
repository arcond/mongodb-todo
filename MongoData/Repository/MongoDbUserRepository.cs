using Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;

namespace MongoData.Repository
{
	public class MongoDbUserRepository :IUserRepository
	{
		private readonly IContext _context;

		public MongoDbUserRepository(IContext context)
		{
			_context = context;
		}

		public IQueryable<IUser> GetAll()
		{
			return GetCollection().FindAll().AsQueryable();
		}

		public IUser Get(object id)
		{
			var query = Query.EQ("_id", (ObjectId)id);
			var user = GetCollection().FindOne(query);
			return user;
		}

		public IUser Add(IUser user)
		{
			GetCollection().Insert((MongoDbUser)user);
			return user;
		}

		public IUser Update(IUser user)
		{
			GetCollection().Save((MongoDbUser)user);
			return user;
		}

		public void Delete(object id)
		{
			var query = Query.EQ("_id", (ObjectId)id);
			GetCollection().Remove(query);
		}

		private MongoCollection<MongoDbUser> GetCollection()
		{
			return _context.Database.GetCollection<MongoDbUser>(Collections.USERS);
		}
	}
}