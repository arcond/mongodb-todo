using MongoDB.Bson;

namespace Domain.Aggregates
{
	public interface ITaskFactory
	{
		Task CreateTask(ObjectId userId, string description);
	}
}