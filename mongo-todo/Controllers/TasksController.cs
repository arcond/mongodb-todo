using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mongo_todo.Models;

namespace mongo_todo.Controllers
{
    public class TasksController : ApiController
	{
		public IEnumerable<TaskModel> Get()
		{
			throw new NotImplementedException();
		}

		public Object Get(int id)
		{
			throw new NotImplementedException();
		}

		public HttpResponseMessage Put(TaskModel task)
		{
			throw new NotImplementedException();
		}

		public Object Post(TaskModel task)
		{
			throw new NotImplementedException();
		}

		public HttpResponseMessage Delete(TaskModel task)
		{
			throw new NotImplementedException();
		}
    }
}
