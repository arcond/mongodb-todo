using MongoDB.Bson;

namespace Domain.Model
{
	public abstract class MongoDbModel
	{
		public ObjectId Id { get; internal set; }
		public BsonDateTime LastModified { get; internal set; }
	}
}