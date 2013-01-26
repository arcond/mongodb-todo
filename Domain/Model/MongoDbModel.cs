using MongoDB.Bson;

namespace Domain.Model
{
	public abstract class MongoDbModel
	{
		private IContext _context;

		public ObjectId Id { get; internal set; }

		public void Save()
		{
			if (Id.Equals(ObjectId.Empty)) Add();
			else Update();
		}

		internal virtual void SetContext(IContext context)
		{
			_context = context;
		}
		
		private void Add()
		{
			var collection = _context.Database.GetCollection("users");
			collection.Insert(this);
		}

		private void Update()
		{
			var collection = _context.Database.GetCollection("users");
			collection.Save(this);
		}
	}
}
