namespace Domain.Aggregates
{
	public interface IUserDependency
	{
		ITaskFactory TaskFactory { get; }
		ITaskRepository TaskRepository { get; }
	}
}