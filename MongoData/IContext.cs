using MongoDB.Driver;

namespace MongoData
{
	public interface IContext
	{
		MongoDatabase Database { get; }
	}
}