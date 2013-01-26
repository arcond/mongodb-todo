using MongoDB.Bson;
using System.Collections.Generic;

namespace Domain.Factory
{
	public class UserFactory :IUserFactory
	{
		public User CreateUser(string name)
		{
			return new User {
				Id = ObjectId.Empty,
				Name = name,
				Tasks = new List<Task>()
			};
		}
	}
}
