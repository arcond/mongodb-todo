using Domain.Model;
using MongoDB.Bson;
using System.Linq;

namespace Domain
{
	public interface IUserRepository
	{
		IQueryable<User> GetAll();
		User Get(ObjectId id);
	}
}
