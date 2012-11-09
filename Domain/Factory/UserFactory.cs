using Domain.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Factory
{
	public class UserFactory :IUserFactory
	{
		public User CreateUser(string name)
		{
			return new User {
				Id = ObjectId.GenerateNewId(DateTime.UtcNow),
				Name = name,
				Tasks = new List<Task>()
			};
		}
	}
}
