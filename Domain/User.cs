using Domain.Model;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
	public class User :MongoDbModel
	{
		//internal BsonArray tasks = new BsonArray();

		public string Name { get; internal set; }
		public IList<Task> Tasks { get; internal set; }

		public void SetName(string name)
		{
			Name = name;
		}

		public void AddTask(Task task)
		{
			Tasks.Add(task);
			//tasks.Add(BsonValue.Create(tasks));
		}

		public void UpdateTask(ObjectId taskId, string description, bool isComplete)
		{
			var existing = Tasks.FirstOrDefault(x => x.Id.Equals(taskId));
			existing.SetDescription(description);
			if (existing.Completed != isComplete) existing.Toggle();
			//tasks[tasks.IndexOf(BsonValue.Create(existing))] = BsonValue.Create(existing);
		}

		public void RemoveTask(Task task)
		{
			Tasks.Remove(task);
			//tasks.Remove(BsonValue.Create(task));
		}
	}
}
