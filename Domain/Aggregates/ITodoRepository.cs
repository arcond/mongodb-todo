using System.Linq;

namespace Domain.Aggregates
{
	public interface ITodoRepository
	{
		IQueryable<ITodo> GetAll(object userId);
		ITodo Get(object id);
		ITodo Add(ITodo todo);
		ITodo Update(ITodo todo);
		void Delete(object id);
	}
}