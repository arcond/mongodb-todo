using Domain.Model;
using MongoDB.Bson;

namespace Domain
{
	public class Task :MongoDbModel
	{
		public string Description { get; internal set; }
		public bool Completed { get; internal set; }
		public ObjectId UserId { get; internal set; }

		public void SetDescription(string description)
		{
			Description = description;
		}

		public void Toggle()
		{
			Completed = !Completed;
		}
	}
}
