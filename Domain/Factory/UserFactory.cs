
namespace Domain.Factory
{
	public class UserFactory :IUserFactory
	{
		public virtual IUser CreateUser(string name)
		{
			return new User {
				Id = default(int),
				Name = name
			};
		}
	}
}