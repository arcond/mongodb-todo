using AutoMapper;
using Domain;
using mongo_todo.Models;
using MongoDB.Bson;
using System;
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

		public HttpResponseMessage GetAll(string userId)
		{
			TaskModel[] tasks = null;

			try {
				tasks = Mapper.Map<TaskModel[]>(_taskRepository.GetAll(ObjectId.Parse(userId)));
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK, tasks);
			response.Headers.Add("Type", typeof(TaskModel[]).Name);
			return response;
		}

		public HttpResponseMessage Get(string userId, string id)
		{

			TaskModel task = null;

			try {
				task = Mapper.Map<TaskModel>(_taskRepository.Get(ObjectId.Parse(id)));
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK, task);
			response.Headers.Add("Type", typeof(TaskModel).Name);
			return response;
		}

		public HttpResponseMessage Put(TaskModel task)
		{
			Task todo = null;

			try {
				todo = _taskRepository.Get(ObjectId.Parse(task.Id));
				todo.SetDescription(task.Description);
				if (task.Completed != todo.Completed) todo.Toggle();
				_taskRepository.Update(todo);
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Type", typeof(TaskModel).Name);
			return response;
		}

		public HttpResponseMessage Post(TaskModel task)
		{
			var routeData = this.Request.GetRouteData();
			var userId = routeData.Values["userId"].ToString();
			var todo = _taskFactory.CreateTask(ObjectId.Parse(userId), task.Description);

			try {
				_taskRepository.Add(todo);

				var user = _userRepository.Get(ObjectId.Parse(userId));
				user.AddTask(todo);
				user = _userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.Created);
			response.Headers.Location = new Uri(string.Format("{0}/{1}", this.Request.RequestUri.AbsoluteUri, todo.Id));
			response.Headers.Add("Type", typeof(TaskModel).Name);
			return response;
		}

		public HttpResponseMessage Delete(TaskModel task)
		{
			try {
				var taskId = ObjectId.Parse(task.Id);
				var todo = _taskRepository.Get(taskId);
				var user = _userRepository.Get(ObjectId.Empty);
				user.RemoveTask(todo);
				_userRepository.Update(user);
				_taskRepository.Delete(taskId);
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			return this.Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}
