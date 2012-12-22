using AutoMapper;
using Domain;
using mongo_todo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mongo_todo.Controllers
{
    public class TasksController : ApiController
	{
		private readonly ITaskRepository _taskRepository;
		private readonly IUserRepository _userRepository;
		private readonly ITaskFactory _taskFactory;
		public TasksController(ITaskRepository taskRepository, IUserRepository userRepository, ITaskFactory taskFactory)
		{
			_taskRepository = taskRepository;
			_userRepository = userRepository;
			_taskFactory = taskFactory;
		}

		public IEnumerable<TaskModel> GetAll(string userId)
		{
			var tasks = _taskRepository.GetAll(ObjectId.Parse(userId));
			var models = Mapper.Map<IEnumerable<TaskModel>>(tasks);
			return models;
		}

		public TaskModel Get(string userId, string id)
		{
			var task = _taskRepository.Get(ObjectId.Parse(id));
			var model = Mapper.Map<TaskModel>(task);
			return model;
		}

		public HttpResponseMessage Put(TaskModel task)
		{
			try {
				var user = _userRepository.GetAll().First(x => x.Tasks.Any(y => y.Id.Equals(ObjectId.Parse(task.Id))));
				user.UpdateTask(ObjectId.Parse(task.Id), task.Description, task.Completed);
			} catch (NullReferenceException) {
				return new HttpResponseMessage(HttpStatusCode.Gone);
			} catch {
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		public TaskModel Post(TaskModel task)
		{
			var todo = _taskFactory.CreateTask(task.Description);
			var user = _userRepository.Get(ObjectId.Parse(task.UserId));
			user.AddTask(todo);

			return Mapper.Map<TaskModel>(todo);
		}

		public HttpResponseMessage Delete(TaskModel task)
		{
			try {
				var taskId = ObjectId.Parse(task.Id);
				var user = _userRepository.GetAll().First(x => x.Tasks.Any(y => y.Id.Equals(taskId)));
				var todo = user.Tasks.First(x => x.Id.Equals(taskId));
				user.RemoveTask(todo);
			} catch (NullReferenceException) {
				return new HttpResponseMessage(HttpStatusCode.Gone);
			} catch {
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
    }
}
