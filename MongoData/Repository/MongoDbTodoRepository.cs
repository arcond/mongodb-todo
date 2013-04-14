using Domain;
using Domain.Aggregates;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;

namespace MongoData.Repository
{
	public class MongoDbTodoRepository :ITodoRepository
	{
		private readonly IContext _context;

		public MongoDbTodoRepository(IContext context)
		{
			_context = context;
		}

		public IQueryable<ITodo> GetAll(object userId)
		{
			var query = Query.EQ("UserId", (ObjectId)userId);
			var tasks = GetCollection().Find(query);
			return tasks.AsQueryable();
		}

		public ITodo Get(object id)
		{
			var query = Query.EQ("_id", (ObjectId)id);
			var task = GetCollection().FindOne(query);
			return task;
		}

		public ITodo Add(ITodo todo)
		{
			GetCollection().Insert((MongoDbTodo)todo);
			return todo;
		}

		public ITodo Update(ITodo todo)
		{
			GetCollection().Save((MongoDbTodo)todo);
			return todo;
		}

		public void Delete(object id)
		{
			var query = Query.EQ("_id", (ObjectId)id);
			GetCollection().Remove(query);
		}

		private MongoCollection<MongoDbTodo> GetCollection()
		{
			return _context.Database.GetCollection<MongoDbTodo>(Collections.TODOS);
		}
	}
}