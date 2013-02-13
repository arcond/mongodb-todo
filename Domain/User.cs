using Domain.Model;
using MongoDB.Bson;
using System;
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
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			Name = name;
		}

		public void AddTask(Task task)
		{
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			if (!Tasks.Contains(task.Id)) Tasks.Add(task.Id);
		}

		public void RemoveTask(Task task)
		{
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			if (Tasks.Contains(task.Id)) Tasks.Remove(task.Id);
		}
	}
}
