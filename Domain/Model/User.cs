using System.Collections.Generic;

namespace Domain.Model
{
	public class User :MongoDbModel
	{
		public string Name { get; internal set; }
		public ICollection<Task> Tasks { get; internal set; }
	}
}
