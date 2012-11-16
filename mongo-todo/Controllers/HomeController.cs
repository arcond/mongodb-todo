using System.Web.Mvc;

namespace mongo_todo.Controllers
{
	public class HomeController :Controller
	{
		public ViewResult Index()
		{
			return View();
		}

	}
}
