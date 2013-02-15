using Domain.Aggregates;
using Domain.Model;
using Microsoft.Practices.Unity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
	public class User :MongoDbModel
	{
		//private readonly UnityContainer _container;
		public User()
		{
			Tasks = new List<ObjectId>();
			this.ResolveDependency();
		}

		public string Name { get; internal set; }
		internal IList<ObjectId> Tasks { get; set; }

		[Dependency, BsonIgnore]
		public IUserDependency Dependency { get; set; }

		public void SetName(string name)
		{
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			Name = name;
		}

		public Task[] GetTasks()
		{
			return Dependency.TaskRepository.GetAll(this.Id).ToArray();
		}

		public Task GetTask(ObjectId id)
		{
			return Dependency.TaskRepository.Get(id);
		}

		public Task AddTask(string description)
		{
			var task = Dependency.TaskFactory.CreateTask(this.Id, description);
			Dependency.TaskRepository.Add(task);
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			Tasks.Add(task.Id);

			return task;
		}

		public void UpdateTask(ObjectId id, string description, bool completed)
		{
			var task = GetTask(id);
			if (!task.Description.Equals(description, StringComparison.CurrentCultureIgnoreCase))
				task.SetDescription(description);

			if (task.Completed != completed) task.Toggle();

			Dependency.TaskRepository.Update(task);
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
		}

		public void RemoveTask(ObjectId id)
		{
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			if (Tasks.Contains(id)) Tasks.Remove(id);
			Dependency.TaskRepository.Delete(id);
		}
	}
}
