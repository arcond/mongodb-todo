using Domain.Model;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Repository
{
	public class DummyTaskRepository :ITaskRepository
	{
		public IQueryable<Task> GetAll(ObjectId userId)
		{
			var tasks = new List<Task>();
			tasks.Add(new Task {
				Completed = false,
				Description = "Task 1",
				Id = ObjectId.GenerateNewId()
			});
			tasks.Add(new Task {
				Completed = false,
				Description = "Task 2",
				Id = ObjectId.GenerateNewId()
			});
			tasks.Add(new Task {
				Completed = false,
				Description = "Task 3",
				Id = ObjectId.GenerateNewId()
			});
			return tasks.AsQueryable();
		}

		Task Get(ObjectId id)
		{
			return new Task {
				Completed = false,
				Description = "Specific Task",
				Id = id
			};
		}
	}
}
