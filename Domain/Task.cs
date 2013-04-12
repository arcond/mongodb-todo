using Domain.Model;
using MongoDB.Bson;
using System;

namespace Domain
{
	public class Task :MongoDbModel
	{
		public string Description { get; internal set; }
		public bool Completed { get; internal set; }
		public ObjectId UserId { get; internal set; }

		public void SetDescription(string description)
		{
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			Description = description;
		}

		public void Toggle()
		{
			LastModified = BsonDateTime.Create(DateTime.UtcNow);
			Completed = !Completed;
		}
	}
}