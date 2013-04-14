using Domain;
using Domain.Aggregates;
using MongoDB.Bson;

namespace MongoData.Factory
{
	public class MongoDbTodoFactory :ITodoFactory
	{
		public ITodo CreateTodo(object userId, string description)
		{
			return new MongoDbTodo {
				Id = new ObjectId(),
				UserId = (ObjectId)userId,
				Completed = false,
				Description = description
			};
		}
	}
}