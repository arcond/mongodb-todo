using System;

namespace Domain
{
	public interface IUser
	{
		object Id { get; }
		DateTime Timestamp { get; }
		string Name { get; }
		void SetName(string name);
		ITodo[] GetTodos();
		ITodo GetTodo(object id);
		ITodo AddTodo(string description);
		void UpdateTodo(object id, string description, bool completed);
		void RemoveTodo(object id);
		void SetTodos(ITodo[] todos);
		void RemoveAllTodos();
		void AddTodos(ITodo[] todos);
	}
}