using System;

namespace Domain.Model
{
	public abstract class DomainModel
	{
		public object Id { get; internal set; }
		public DateTime Timestamp { get; internal set; }
	}
}
