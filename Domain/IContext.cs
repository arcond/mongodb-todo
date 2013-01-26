using MongoDB.Driver;

namespace Domain
{
	public interface IContext
	{
		MongoDatabase Database { get; }
	}
}
