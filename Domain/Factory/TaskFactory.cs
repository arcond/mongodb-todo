using MongoDB.Bson;
using System;

namespace Domain.Factory
{
	public class TaskFactory :ITaskFactory
	{
		public Task CreateTask(string description)
		{
			return new Task {
				Completed = false,
				Description = description,
				Id = ObjectId.GenerateNewId(DateTime.Now)
			};
		}
	}
}
