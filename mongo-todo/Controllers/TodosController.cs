using AutoMapper;
using Domain;
using mongo_todo.Models;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mongo_todo.Controllers
{
	public class TodosController :ApiController
	{
		private readonly IUserRepository _userRepository;

		public TodosController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public HttpResponseMessage GetAll(string userId)
		{
			TodoModel[] models;

			try {
				models =
					Mapper.Map<TodoModel[]>(
						_userRepository.Get(ObjectId.Parse(userId))
							.GetTodos());
			} catch (NullReferenceException ex) {
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = Request.CreateResponse(
				HttpStatusCode.OK,
				models);
			response.Headers.Add("Type", typeof(TodoModel[]).Name);
			return response;
		}

		public HttpResponseMessage Get(string userId, string id)
		{
			TodoModel model;

			try {
				model =
					Mapper.Map<TodoModel>(
						_userRepository.Get(ObjectId.Parse(userId))
							.GetTodo(ObjectId.Parse(id)));
			} catch (NullReferenceException ex) {
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = Request.CreateResponse(
				HttpStatusCode.OK, model);
			response.Headers.Add("Type", typeof(TodoModel).Name);
			return response;
		}

		public HttpResponseMessage Put(string userId, string id, TodoModel todo)
		{
			try {
				var user = _userRepository.Get(ObjectId.Parse(userId));
				user.UpdateTodo(ObjectId.Parse(todo.Id), todo.Description, todo.Completed);
				_userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Type", typeof(TodoModel).Name);
			return response;
		}

		public HttpResponseMessage Put(string userId, TodoModel[] models)
		{
			if (models == null || models.Any(x => string.IsNullOrWhiteSpace(x.Id)))
				return Request.CreateResponse(HttpStatusCode.BadRequest);

			try {
				var user = _userRepository.Get(ObjectId.Parse(userId));
				var todos = user.GetTodos();
				todos = models.Aggregate(
					todos,
					(current, model) =>
						current.Where(
							x =>
								x.Id.Equals(ObjectId.Parse(model.Id))
							).ToArray()
					);
				user.SetTodos(todos);
				_userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Type", typeof(TodoModel[]).Name);
			return response;
		}

		public HttpResponseMessage Patch(string userId, TodoModel[] models)
		{
			if (models == null || models.Any(x => string.IsNullOrWhiteSpace(x.Id)))
				return Request.CreateResponse(HttpStatusCode.BadRequest);

			try {
				var user = _userRepository.Get(ObjectId.Parse(userId));
				foreach (var model in models) {
					user.UpdateTodo(
						ObjectId.Parse(model.Id),
						model.Description,
						model.Completed);
				}
				_userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Type", typeof(TodoModel[]).Name);
			return response;
		}

		public HttpResponseMessage Post(string userId, TodoModel model)
		{
			ITodo todo;

			try {
				var user = _userRepository.Get(ObjectId.Parse(userId));
				todo = user.AddTodo(model.Description);
				_userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			var response = Request.CreateResponse(HttpStatusCode.Created);
			response.Headers.Location =
				new Uri(string.Format("{0}/{1}", Request.RequestUri.AbsoluteUri, todo.Id));
			response.Headers.Add("Type", typeof(TodoModel).Name);
			return response;
		}

		public HttpResponseMessage Delete(string userId, string id)
		{
			try {
				var todoId = ObjectId.Parse(id);
				var user = _userRepository.Get(ObjectId.Parse(userId));
				user.RemoveTodo(todoId);
				_userRepository.Update(user);
			} catch (NullReferenceException ex) {
				return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
			} catch (Exception ex) {
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}