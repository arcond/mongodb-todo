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

		[BsonElement]
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
			Tasks.Add(task.Id);
			LastModified = BsonDateTime.Create(DateTime.UtcNow);

			return task;
		}

		public void UpdateTask(ObjectId id, string description, bool completed)
		{
			var task = GetTask(id);
			if (
				!task.Description.Equals(
					description, StringComparison.CurrentCultureIgnoreCase))
				task.SetDescription(description);

			if (task.Completed != completed) task.Toggle();

			Dependency.TaskRepository.Update(task);
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
		}

		public void RemoveTask(ObjectId id)
		{
			if (Tasks.Contains(id)) Tasks.Remove(id);
			Dependency.TaskRepository.Delete(id);
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
		}

		public void SetTasks(Task[] todos)
		{
			if (todos == null
				|| todos.Any(x => x.Id == null || x.Id.Equals(ObjectId.Empty))) {
				throw new ArgumentNullException(
					"todos",
					"Cannot add new tasks with no or default ID, please use the AddTask method first.");
			}

			if (!todos.Any(x => x.UserId.Equals(Id) || x.UserId.Equals(ObjectId.Empty)))
				throw new ArgumentException("Cannot add tasks that belong to another user");

			RemoveAllTasks();
			AddTasks(todos);
			LastModified = BsonDateTime.Create(DateTime.Now);
		}

		private void RemoveAllTasks()
		{
			foreach (var todoId in Tasks) {
				Tasks.Remove(todoId);
				Dependency.TaskRepository.Delete(todoId);
			}
		}

		private void AddTasks(Task[] todos)
		{
			foreach (var todo in todos) {
				Dependency.TaskRepository.Add(todo);
				Tasks.Add(todo.Id);
			}
		}
	}
}