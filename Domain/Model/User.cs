using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
	public class User :MongoDbModel
	{
		public string Name { get; internal set; }
		public ICollection<Task> Tasks { get; internal set; }

		public void SetName(string name)
		{
			Name = name;
		}

		public void AddTask(Task task)
		{
			Tasks.Add(task);
		}

		public void UpdateTask(ObjectId taskId, string description, bool isComplete)
		{
			var existing = Tasks.FirstOrDefault(x => x.Id.Equals(taskId));
			existing.SetDescription(description);
			if (existing.Completed != isComplete) existing.Toggle();
		}

		public void RemoveTask(Task task)
		{
			Tasks.Remove(task);
		}
	}
}
