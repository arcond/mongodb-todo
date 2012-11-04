
namespace Domain.Model
{
	public class Task :MongoDbModel
	{
		public string Description { get; internal set; }
		public bool Completed { get; internal set; }
	}
}
