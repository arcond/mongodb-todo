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
				Id = DummyGlobal.Instance.BaseTaskObjectId[0]
			});
			tasks.Add(new Task {
				Completed = false,
				Description = "Task 2",
				Id = DummyGlobal.Instance.BaseTaskObjectId[1]
			});
			tasks.Add(new Task {
				Completed = false,
				Description = "Task 3",
				Id = DummyGlobal.Instance.BaseTaskObjectId[2]
			});
			return tasks.AsQueryable();
		}

		public Task Get(ObjectId id)
		{
			return new Task {
				Completed = false,
				Description = "Specific Task",
				Id = id
			};
		}


		public Task Add(Task task)
		{
			throw new System.NotImplementedException();
		}

		public Task Update(Task task)
		{
			throw new System.NotImplementedException();
		}

		public void Delete(ObjectId id)
		{
			throw new System.NotImplementedException();
		}
	}
}
