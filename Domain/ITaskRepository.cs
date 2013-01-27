using MongoDB.Bson;
using System.Linq;

namespace Domain
{
	public interface ITaskRepository
	{
		IQueryable<Task> GetAll(ObjectId userId);
		Task Get(ObjectId id);
	}
}
