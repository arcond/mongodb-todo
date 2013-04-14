namespace Domain.Aggregates
{
	public class UserDependency :IUserDependency
	{
		public UserDependency(
			ITodoFactory todoFactory, ITodoRepository todoRepository)
		{
			TodoFactory = todoFactory;
			TodoRepository = todoRepository;
		}

		public ITodoFactory TodoFactory { get; private set; }
		public ITodoRepository TodoRepository { get; private set; }
	}
}