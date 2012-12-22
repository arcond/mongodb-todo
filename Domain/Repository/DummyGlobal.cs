
using MongoDB.Bson;
using System.Collections.Generic;
namespace Domain.Repository
{
	public class DummyGlobal
	{
		private static readonly object _pad = new object();
		private static volatile DummyGlobal _instance;
		private DummyGlobal()
		{
			BaseUserObjectId = new List<ObjectId>();
			BaseTaskObjectId = new List<ObjectId>();
		}

		public static DummyGlobal Instance
		{
			get
			{
				if (_instance == null) {
					lock (_pad) {
						if (_instance == null) _instance = new DummyGlobal();
					}
				}
				return _instance;
			}
		}

		public List<ObjectId> BaseUserObjectId { get; set; }
		public List<ObjectId> BaseTaskObjectId { get; set; }
	}
}
