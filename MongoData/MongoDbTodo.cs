using Domain;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace MongoData
{
	internal class MongoDbTodo :ITodo
	{
		private readonly Todo _todo;

		public MongoDbTodo()
		{
			_todo = new Todo();
		}

		[BsonId(IdGenerator = typeof(ObjectIdGenerator))]
		public object Id
		{
			get { return _todo.Id; }
			internal set { _todo.ChangeId(value); }
		}

		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime Timestamp
		{
			get { return _todo.Timestamp; }
		}

		public string Description
		{
			get { return _todo.Description; }
			internal set { _todo.SetDescription(value); }
		}

		public bool Completed
		{
			get { return _todo.Completed; }
			internal set
			{
				var complete = value;
				if (_todo.Completed != complete) _todo.Toggle();
			}
		}

		public object UserId
		{
			get { return _todo.UserId; }
			internal set { _todo.ReassignUserId(value); }
		}

		public void SetDescription(string description)
		{
			_todo.SetDescription(description);
		}

		public void Toggle()
		{
			_todo.Toggle();
		}
	}
}