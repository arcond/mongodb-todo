namespace Domain.Aggregates
{
	public interface IUserDependency
	{
		ITodoFactory TodoFactory { get; }
		ITodoRepository TodoRepository { get; }
	}
}