
namespace Domain.Aggregates
{
	public interface ITodoFactory
	{
		ITodo CreateTodo(object userId, string description);
	}
}