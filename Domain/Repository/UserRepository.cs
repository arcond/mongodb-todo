using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;

namespace Domain.Repository
{
	public class UserRepository :IUserRepository
	{
		private readonly IContext _context;
		public UserRepository(IContext context)
		{
			_context = context;
		}

		public IQueryable<User> GetAll()
		{
			var users = GetCollection().FindAll();
			return users.AsQueryable();
		}

		public User Get(ObjectId id)
		{
			var query = Query.EQ("_id", id);
			var user = GetCollection().FindOne(query);
			return user;
		}

		public User Add(User user)
		{
			GetCollection().Insert(user);
			return user;
		}

		public User Update(User user)
		{
			GetCollection().Save(user);
			return user;
		}

		public void Delete(ObjectId id)
		{
			var query = Query.EQ("_id", id);
			GetCollection().Remove(query);
		}

		private MongoCollection<User> GetCollection()
		{
			return _context.Database.GetCollection<User>("users");
		}
	}
}
