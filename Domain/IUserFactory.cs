namespace Domain
{
	public interface IUserFactory
	{
		IUser CreateUser(string name);
	}
}