using Domain;
using mongo_todo.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace mongo_todo.Controllers
{
    public class TasksController : ApiController
	{
		private readonly ITaskRepository _taskRepository;
		public TasksController(ITaskRepository taskRepository)
		{
			_taskRepository = taskRepository;
		}

		public IEnumerable<TaskModel> Get()
		{
			throw new NotImplementedException();
		}

		public Object Get(string id)
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
