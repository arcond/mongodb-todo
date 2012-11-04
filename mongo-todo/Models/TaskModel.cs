using System;
using System.ComponentModel.DataAnnotations;

namespace mongo_todo.Models
{
	public class TaskModel
	{
		public Object Id { get; set; }

		[Required]
		public string Description { get; set; }

		public bool Completed { get; set; }
	}
}