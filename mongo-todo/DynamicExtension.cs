using System.Collections.Generic;
using System.Dynamic;

namespace mongo_todo
{
	public static class DynamicExtension
	{
		public static bool HasProperty(this ExpandoObject obj, string propertyName)
		{
			var kvp = (IDictionary<string, object>)obj;
			return kvp.ContainsKey(propertyName);
		}
	}
}