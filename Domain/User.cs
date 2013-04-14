using System;
using System.Linq;
using Domain.Aggregates;
using Domain.Model;
using Microsoft.Practices.Unity;

namespace Domain
{
	public class User :DomainModel, IUser
	{
		public User()
		{
			this.ResolveDependency();
		}

		[Dependency]
		public IUserDependency Dependency { get; set; }

		public string Name { get; internal set; }

		public void SetName(string name)
		{
			Timestamp = DateTime.UtcNow;
			Name = name;
		}

		public ITodo[] GetTodos()
		{
			return Dependency.TodoRepository.GetAll(this.Id).ToArray();
		}

		public ITodo GetTodo(object id)
		{
			return Dependency.TodoRepository.Get(id);
		}

		public virtual ITodo AddTodo(string description)
		{
			var task = Dependency.TodoFactory.CreateTodo(this.Id, description);
			Dependency.TodoRepository.Add(task);
			Timestamp = DateTime.UtcNow;

			return task;
		}

		public void UpdateTodo(object id, string description, bool completed)
		{
			var task = GetTodo(id);
			if (
				!task.Description.Equals(
					description, StringComparison.CurrentCultureIgnoreCase))
				task.SetDescription(description);

			if (task.Completed != completed) task.Toggle();

			Dependency.TodoRepository.Update(task);
			Timestamp = DateTime.UtcNow;
		}

		public virtual void RemoveTodo(object id)
		{
			Dependency.TodoRepository.Delete(id);
			Timestamp = DateTime.UtcNow;
		}

		public void SetTodos(ITodo[] todos)
		{
			if (todos == null
				|| todos.Any(x => x.Id == null)) {
				throw new ArgumentNullException(
					"todos",
					"Cannot add new tasks with no or default ID, please use the AddTodo method first.");
			}

			if (!todos.Any(x => x.UserId.Equals(Id)))
				throw new ArgumentException("Cannot add tasks that belong to another user");

			RemoveAllTodos();
			AddTodos(todos);
			Timestamp = DateTime.Now;
		}

		public void RemoveAllTodos()
		{
			foreach (var todo in GetTodos()) Dependency.TodoRepository.Delete(todo.Id);
		}

		public void AddTodos(ITodo[] todos)
		{
			foreach (var todo in todos) Dependency.TodoRepository.Add(todo);
		}

		public void ChangeId(object newId)
		{
			Timestamp = DateTime.UtcNow;
			Id = newId;
		}
	}
}