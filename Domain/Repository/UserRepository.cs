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
			users.ToList().ForEach(x => x.SetContext(_context));
			return users.AsQueryable();
		}

		public User Get(ObjectId id)
		{
			var query = Query.EQ("_id", id);
			var user = GetCollection().FindOne(query);
			user.SetContext(_context);
			return user;
		}

		private MongoCollection<User> GetCollection()
		{
			return _context.Database.GetCollection<User>("users");
		}
	}
}
