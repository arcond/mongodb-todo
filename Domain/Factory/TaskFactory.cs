using MongoDB.Bson;

namespace Domain.Factory
{
	public class TaskFactory :ITaskFactory
	{
		public Task CreateTask(ObjectId userId, string description)
		{
			return new Task {
				Completed = false,
				Description = description,
				Id = ObjectId.Empty,
				UserId = userId
			};
		}
	}
}
