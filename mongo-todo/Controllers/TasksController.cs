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
		private readonly IUserRepository _userRepository;
		public TasksController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public HttpResponseMessage GetAll(string userId)
		{
			TaskModel[] tasks = null;

			try {
				tasks = Mapper.Map<TaskModel[]>(_userRepository.Get(ObjectId.Parse(userId)).GetTasks());
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
				task = Mapper.Map<TaskModel>(_userRepository.Get(ObjectId.Parse(userId)).GetTask(ObjectId.Parse(id)));
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK, task);
			response.Headers.Add("Type", typeof(TaskModel).Name);
			return response;
		}

		public HttpResponseMessage Put(string userId, TaskModel task)
		{
			try {
				var user = _userRepository.Get(ObjectId.Parse(userId));
				user.UpdateTask(ObjectId.Parse(task.Id), task.Description, task.Completed);
				_userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Type", typeof(TaskModel).Name);
			return response;
		}

		public HttpResponseMessage Post(string userId, TaskModel task)
		{
			Task todo = null;

			try {
				var user = _userRepository.Get(ObjectId.Parse(userId));
				todo = user.AddTask(task.Description);
				_userRepository.Update(user);
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

		public HttpResponseMessage Delete(string userId, string id)
		{
			try {
				var todoId = ObjectId.Parse(id);
				var user = _userRepository.Get(ObjectId.Parse(userId));
				user.RemoveTask(todoId);
				_userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			return this.Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}
