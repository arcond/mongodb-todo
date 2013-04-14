using System.Linq;

namespace Domain
{
	public interface IUserRepository
	{
		IQueryable<IUser> GetAll();
		IUser Get(object id);
		IUser Add(IUser user);
		IUser Update(IUser user);
		void Delete(object id);
	}
}