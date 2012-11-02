using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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