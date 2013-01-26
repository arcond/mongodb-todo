using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;

namespace Domain.Repository
{
	public class TaskRepository :ITaskRepository
	{
		private readonly IContext _context;
		public TaskRepository(IContext context)
		{
			_context = context;
		}

		public IQueryable<Task> GetAll(ObjectId userId)
		{
			var query = Query.EQ("_id", userId);
			var user = GetUserCollection().FindOne(query);
			user.SetContext(_context);
			return user.Tasks.AsQueryable();
		}

		public Task Get(ObjectId id)
		{
			var query = Query.EQ("_id", id);
			var task = GetTaskCollection().FindOne(query);
			task.SetContext(_context);
			return task;
		}

		private MongoCollection<User> GetUserCollection()
		{
			return _context.Database.GetCollection<User>("users");
		}

		private MongoCollection<Task> GetTaskCollection()
		{
			return _context.Database.GetCollection<Task>("tasks");
		}
	}
}
