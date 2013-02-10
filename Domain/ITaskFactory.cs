using MongoDB.Bson;

namespace Domain
{
	public interface ITaskFactory
	{
		Task CreateTask(ObjectId userId, string description);
	}
}
