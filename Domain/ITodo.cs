using System;

namespace Domain
{
	public interface ITodo
	{
		object Id { get; }
		DateTime Timestamp { get; }
		string Description { get; }
		bool Completed { get; }
		object UserId { get; }
		void SetDescription(string description);
		void Toggle();
	}
}