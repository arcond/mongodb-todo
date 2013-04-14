using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoData
{
	internal class MongoDbUser :IUser
	{
		private readonly User _user;

		public MongoDbUser()
		{
			_user = new User();
			Todos = new List<ObjectId>();
		}

		public IList<ObjectId> Todos { get; set; }

		[BsonId(IdGenerator = typeof(ObjectIdGenerator))]
		public object Id
		{
			get { return _user.Id; }
			internal set { _user.ChangeId(value); }
		}

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime Timestamp
		{
			get { return _user.Timestamp; }
		}

		public string Name
		{
			get { return _user.Name; }
			internal set { _user.SetName(value); }
		}

		public void SetName(string name)
		{
			_user.SetName(name);
		}

		public ITodo[] GetTodos()
		{
			return _user.GetTodos();
		}

		public ITodo GetTodo(object id)
		{
			return _user.GetTodo(id);
		}

		public ITodo AddTodo(string description)
		{
			var todo = _user.AddTodo(description);
			Todos.Add((ObjectId)todo.Id);
			return todo;
		}

		public void UpdateTodo(object id, string description, bool completed)
		{
			_user.UpdateTodo(id, description, completed);
		}

		public void RemoveTodo(object id)
		{
			_user.RemoveTodo(id);
			Todos.Remove((ObjectId)id);
		}

		public void SetTodos(ITodo[] todos)
		{
			if (todos == null
				|| todos.Any(x => x.Id == null || ((ObjectId)x.Id).Equals(ObjectId.Empty))) {
				throw new ArgumentNullException(
					"todos",
					"Cannot add new tasks with no or default ID, please use the AddTodo method first.");
			}

			if (
				!todos.Any(
					x =>
						((ObjectId)x.UserId).Equals((ObjectId)Id)
							|| ((ObjectId)x.UserId).Equals(ObjectId.Empty)))
				throw new ArgumentException("Cannot add tasks that belong to another user");

			_user.SetTodos(todos);
		}

		public void RemoveAllTodos()
		{
			_user.RemoveAllTodos();
		}

		public void AddTodos(ITodo[] todos)
		{
			_user.AddTodos(todos);
			Todos.Clear();
			foreach (var todo in _user.GetTodos()) Todos.Add((ObjectId)todo.Id);
		}
	}
}