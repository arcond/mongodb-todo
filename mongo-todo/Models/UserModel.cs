using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mongo_todo.Models
{
	public class UserModel
	{
		public string Id { get; set; }

		[Required]
		public string Name { get; set; }

		public IEnumerable<TaskModel> Tasks { get; set; }
	}
}