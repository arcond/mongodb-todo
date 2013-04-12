using MongoDB.Bson;
using System.Linq;

namespace Domain
{
	public interface IUserRepository
	{
		IQueryable<User> GetAll();
		User Get(ObjectId id);
		User Add(User user);
		User Update(User user);
		void Delete(ObjectId id);
	}
}