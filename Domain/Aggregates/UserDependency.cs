
namespace Domain.Aggregates
{
	public class UserDependency :IUserDependency
	{
		public UserDependency(ITaskFactory taskFactory, ITaskRepository taskRepository)
		{
			TaskFactory = taskFactory;
			TaskRepository = taskRepository;
		}

		public ITaskFactory TaskFactory { get; private set; }
		public ITaskRepository TaskRepository { get; private set; }
	}
}
