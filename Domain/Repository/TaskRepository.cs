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
			var user = GetCollection().FindOne(query);
			return user.Tasks.AsQueryable();
		}

		public Task Get(ObjectId id)
		{
			var query = Query.EQ("tasks._id", id);
			var task = GetCollection().FindOne(query).Tasks.FirstOrDefault(x => x.Id.Equals(id));
			return task;
		}

		public Task Add(Task task)
		{
			var query = Query.EQ("tasks._id", task.Id);
			var user = GetCollection().FindOne(query);
			user.AddTask(task);
			GetCollection().Save(user);
			return task;
		}

		public Task Update(Task task)
		{
			var query = Query.EQ("tasks._id", task.Id);
			var user = GetCollection().FindOne(query);
			var todo = user.Tasks.FirstOrDefault(x => x.Id.Equals(task.Id));
			todo.SetDescription(task.Description);
			if (task.Completed != todo.Completed) todo.Toggle();
			GetCollection().Save(user);
			return task;
		}

		public void Delete(ObjectId id)
		{
			var query = Query.EQ("tasks._id", id);
			GetCollection().Remove(query);
		}

		private MongoCollection<User> GetCollection()
		{
			return _context.Database.GetCollection<User>("users");
		}
	}
}
