using Domain.Model;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Repository
{
	public class DummyUserRepository :IUserRepository
	{
		private readonly ITaskRepository _taskRepository;
		public DummyUserRepository(ITaskRepository taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public IQueryable<User> GetAll()
		{
			var users = new List<User>();
			var userId = DummyGlobal.Instance.BaseUserObjectId[0];
			users.Add(new User {
				Id = userId,
				Name = "User 1",
				Tasks = _taskRepository.GetAll(userId).ToList()
			});
			userId = DummyGlobal.Instance.BaseUserObjectId[1];
			users.Add(new User {
				Id = userId,
				Name = "User 2",
				Tasks = _taskRepository.GetAll(userId).ToList()
			});
			userId = DummyGlobal.Instance.BaseUserObjectId[2];
			users.Add(new User {
				Id = userId,
				Name = "User 3",
				Tasks = _taskRepository.GetAll(userId).ToList()
			});
			return users.AsQueryable();
		}

		public User Get(ObjectId id)
		{
			if (id == ObjectId.Empty)
				return new User {
					Id = ObjectId.Empty,
					Name = "Taskless User",
					Tasks = new List<Task>()
				};

			return new User {
				Id = id,
				Name = "Specific User",
				Tasks = _taskRepository.GetAll(id).ToList()
			};
		}
	}
}
