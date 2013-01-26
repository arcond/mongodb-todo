using MongoDB.Bson;
using System;

namespace Domain.Factory
{
	public class TaskFactory :ITaskFactory
	{
		private readonly IContext _context;
		public TaskFactory(IContext context)
		{
			_context = context;
		}

		public Task CreateTask(string description)
		{
			var task = new Task {
				Completed = false,
				Description = description,
				Id = ObjectId.GenerateNewId(DateTime.UtcNow)
			};
			task.SetContext(_context);
			return task;
		}
	}
}
