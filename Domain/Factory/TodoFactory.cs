using Domain.Aggregates;

namespace Domain.Factory
{
	public class TodoFactory :ITodoFactory
	{
		public virtual ITodo CreateTodo(object userId, string description)
		{
			return new Todo {
				Completed = false,
				Description = description,
				Id = default(int),
				UserId = userId
			};
		}
	}
}