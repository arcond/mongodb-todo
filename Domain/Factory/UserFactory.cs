using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Domain.Factory
{
	public class UserFactory :IUserFactory
	{
		private readonly IContext _context;
		public UserFactory(IContext context)
		{
			_context = context;
		}

		public User CreateUser(string name)
		{
			var user = new User {
				Id = ObjectId.GenerateNewId(DateTime.UtcNow),
				Name = name,
				Tasks = new List<Task>()
			};
			user.SetContext(_context);
			return user;
		}
	}
}
