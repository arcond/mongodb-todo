using Domain.Model;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Domain
{
	public class User :MongoDbModel
	{
		public User()
		{
			Tasks = new List<ObjectId>();
		}

		public string Name { get; internal set; }
		internal IList<ObjectId> Tasks { get; set; }

		public void SetName(string name)
		{
			Name = name;
		}

		public void AddTask(Task task)
		{
			if (!Tasks.Contains(task.Id)) Tasks.Add(task.Id);
		}

		public void RemoveTask(Task task)
		{
			if (Tasks.Contains(task.Id)) Tasks.Remove(task.Id);
		}
	}
}
