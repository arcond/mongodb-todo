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
			var query = Query.EQ("UserId", userId);
			var tasks = GetCollection().Find(query);
			return tasks.AsQueryable();
		}

		public Task Get(ObjectId id)
		{
			var query = Query.EQ("_id", id);
			var task = GetCollection().FindOne(query);
			return task;
		}

		public Task Add(Task task)
		{
			GetCollection().Insert(task);
			return task;
		}

		public Task Update(Task task)
		{
			GetCollection().Save(task);
			return task;
		}

		public void Delete(ObjectId id)
		{
			var query = Query.EQ("_id", id);
			GetCollection().Remove(query);
		}

		private MongoCollection<Task> GetCollection()
		{
			return _context.Database.GetCollection<Task>("todos");
		}
	}
}
