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
			User user = null;
			try {
				user = _userRepository.GetAll().First(x => x.Tasks.Any(y => y.Id.Equals(ObjectId.Parse(task.Id))));
				user.UpdateTask(ObjectId.Parse(task.Id), task.Description, task.Completed);
				user = _userRepository.Update(user);
			} catch (NullReferenceException) {
				return new HttpResponseMessage(HttpStatusCode.Gone);
			} catch {
				return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		public TaskModel Post(TaskModel task)
		{
			var user = _userRepository.Get(ObjectId.Parse(task.UserId));
			var todo = _taskFactory.CreateTask(task.Description);
			user.AddTask(todo);
			user = _userRepository.Update(user);

			return Mapper.Map<TaskModel>(todo);
		}

		public HttpResponseMessage Delete(TaskModel task)
		{
			try {
				var taskId = ObjectId.Parse(task.Id);
				var user = _userRepository.Get(ObjectId.Parse(task.UserId));
				var todo = _taskRepository.Get(taskId);
				user.RemoveTask(todo);
				_userRepository.Update(user);
			} catch (NullReferenceException) {
				return new HttpResponseMessage(HttpStatusCode.Gone);
			} catch {
				return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
    }
}
