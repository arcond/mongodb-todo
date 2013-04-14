using Domain.Model;
using System;

namespace Domain
{
	public class Todo :DomainModel, ITodo
	{
		public Todo()
		{
			Timestamp = DateTime.UtcNow;
		}

		public string Description { get; internal set; }
		public bool Completed { get; internal set; }
		public object UserId { get; internal set; }

		public void ChangeId(object newId)
		{
			Timestamp = DateTime.UtcNow;
			Id = newId;
		}

		public void SetDescription(string description)
		{
			Timestamp = DateTime.UtcNow;
			Description = description;
		}

		public void Toggle()
		{
			Timestamp = DateTime.UtcNow;
			Completed = !Completed;
		}

		public void ReassignUserId(object newUserId)
		{
			Timestamp = DateTime.UtcNow;
			UserId = newUserId;
		}
	}
}