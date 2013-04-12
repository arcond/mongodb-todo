using AutoMapper;
using Domain;
using mongo_todo.Models;
using MongoDB.Bson;
using System;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mongo_todo.Controllers
{
	public class UsersController :ApiController
	{
		private readonly IUserFactory _userFactory;
		private readonly IUserRepository _userRepository;

		public UsersController(
			IUserRepository userRepository, IUserFactory userFactory)
		{
			_userRepository = userRepository;
			_userFactory = userFactory;
		}

		public HttpResponseMessage Head()
		{
			HttpResponseMessage response = null;
			if (Request.Headers.IfModifiedSince.HasValue) {
				try {
					IQueryable<User> users =
						_userRepository.GetAll()
									   .Where(
											  x =>
											  x.LastModified.AsDateTime
											  > Request.Headers.IfModifiedSince);
					if (users.Any()) {
						users = users.OrderBy(x => x.LastModified);
						response = Request.CreateResponse(HttpStatusCode.OK);
						response.Headers.Add(
											 "Last-Modified",
											 users.First()
												  .LastModified.AsDateTime.ToString(
																				    CultureInfo
																						.InvariantCulture));
					} else response = Request.CreateResponse(HttpStatusCode.NotModified);
				} catch (Exception ex) {
					return Request.CreateResponse(
												  HttpStatusCode.InternalServerError,
												  ex.Message);
				}
			}

			response.Headers.Add("Type", typeof(UserModel[]).Name);
			response.Headers.Add(
								 "Link",
								 this.GetLinkHeader<TaskModel[]>(
																 Request,
																 response,
																 "/tasks"));
			return response;
		}

		public HttpResponseMessage Head(string id)
		{
			HttpResponseMessage response = null;
			if (Request.Headers.IfModifiedSince.HasValue) {
				try {
					User user = _userRepository.Get(ObjectId.Parse(id));
					if (user.LastModified.AsDateTime > Request.Headers.IfModifiedSince.Value) {
						response = Request.CreateResponse(HttpStatusCode.OK);
						response.Headers.Add(
											 "Last-Modified",
											 user.LastModified.AsDateTime.ToString(
																				   CultureInfo
																					   .InvariantCulture));
					}
				} catch (Exception ex) {
					return Request.CreateResponse(
												  HttpStatusCode.InternalServerError,
												  ex.Message);
				}
			}

			if (response == null)
				response = Request.CreateResponse(HttpStatusCode.NotModified);
			response.Headers.Add("Type", typeof(UserModel).Name);
			response.Headers.Add(
								 "Link",
								 this.GetLinkHeader<TaskModel[]>(
																 Request,
																 response,
																 "/tasks"));
			return response;
		}

		public HttpResponseMessage Get()
		{
			UserModel[] users;

			try {
				users = Mapper.Map<UserModel[]>(_userRepository.GetAll().ToArray());
			} catch (Exception ex) {
				return Request.CreateResponse(
											  HttpStatusCode.InternalServerError, ex.Message);
			}

			HttpResponseMessage response = Request.CreateResponse(
																  HttpStatusCode.OK,
																  users);
			response.Headers.Add("Type", typeof(UserModel[]).Name);
			return response;
		}

		public HttpResponseMessage Get(string id)
		{
			UserModel user;

			try {
				user = Mapper.Map<UserModel>(_userRepository.Get(ObjectId.Parse(id)));
			} catch (Exception ex) {
				return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
			}

			HttpResponseMessage response = Request.CreateResponse(
																  HttpStatusCode.OK, user);
			response.Headers.Add(
								 "Link",
								 this.GetLinkHeader<TaskModel[]>(
																 Request,
																 response,
																 "/tasks"));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Put(UserModel user)
		{
			try {
				User savedUser = _userRepository.Get(ObjectId.Parse(user.Id));
				savedUser.SetName(user.Name);
				_userRepository.Update(savedUser);
			} catch (NullReferenceException ex) {
				return Request.CreateResponse(HttpStatusCode.Gone, ex.Message);
			} catch (Exception ex) {
				return Request.CreateResponse(
											  HttpStatusCode.InternalServerError, ex.Message);
			}

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add(
								 "Link",
								 this.GetLinkHeader<TaskModel[]>(
																 Request,
																 response,
																 "/tasks"));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Post(UserModel user)
		{
			User newUser = _userFactory.CreateUser(user.Name);

			try {
				newUser = _userRepository.Add(newUser);
			} catch (Exception ex) {
				return Request.CreateResponse(
											  HttpStatusCode.InternalServerError, ex.Message);
			}

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
			response.Headers.Location =
				new Uri(
					string.Format("{0}/{1}", Request.RequestUri.AbsoluteUri, newUser.Id));
			response.Headers.Add(
								 "Link",
								 this.GetLinkHeader<TaskModel[]>(
																 Request,
																 response,
																 newUser.Id.ToString(),
																 "/tasks"));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Patch(string id, ExpandoObject user)
		{
			try {
				string newName = null;
				if (user.HasProperty("Name")) newName = ((dynamic)user).Name;

				if (newName != null) {
					User savedUser = _userRepository.Get(ObjectId.Parse(id));
					savedUser.SetName(newName);
					_userRepository.Update(savedUser);
				}
			} catch (NullReferenceException ex) {
				return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
			} catch (Exception ex) {
				return Request.CreateResponse(
											  HttpStatusCode.InternalServerError, ex.Message);
			}

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add(
								 "Link",
								 this.GetLinkHeader<TaskModel[]>(
																 Request,
																 response,
																 "/tasks"));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Delete(string id)
		{
			try {
				_userRepository.Delete(ObjectId.Parse(id));
			} catch (NullReferenceException ex) {
				return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
			} catch (Exception ex) {
				return Request.CreateResponse(
											  HttpStatusCode.InternalServerError, ex.Message);
			}

			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}