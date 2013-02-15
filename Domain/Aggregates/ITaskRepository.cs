using MongoDB.Bson;
using System.Linq;

namespace Domain.Aggregates
{
	public interface ITaskRepository
	{
		IQueryable<Task> GetAll(ObjectId userId);
		Task Get(ObjectId id);
		Task Add(Task task);
		Task Update(Task task);
		void Delete(ObjectId id);
	}
}
