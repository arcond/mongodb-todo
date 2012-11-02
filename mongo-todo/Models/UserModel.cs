using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mongo_todo.Models
{
	public class UserModel
	{
		public Object Id { get; set; }

		[Required]
		public string Name { get; set; }

		public IEnumerable<TaskModel> Tasks { get; set; }
	}
}