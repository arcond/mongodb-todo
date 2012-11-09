using Domain.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Factory
{
	public class TaskFactory :ITaskFactory
	{
		public Task CreateTask(string description)
		{
			return new Task {
				Completed = false,
				Description = description,
				Id = ObjectId.GenerateNewId(DateTime.UtcNow)
			};
		}
	}
}
