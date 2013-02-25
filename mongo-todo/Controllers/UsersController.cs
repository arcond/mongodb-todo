using AutoMapper;
using Domain;
using mongo_todo.Models;
using MongoDB.Bson;
using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mongo_todo.Controllers
{
	public class UsersController :ApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IUserFactory _userFactory;
		public UsersController(IUserRepository userRepository, IUserFactory userFactory)
		{
			_userRepository = userRepository;
			_userFactory = userFactory;
		}

		public HttpResponseMessage Head()
		{
			HttpResponseMessage response = null;
			if (this.Request.Headers.IfModifiedSince.HasValue) {
				try {
					var users = _userRepository.GetAll().Where(x => x.LastModified.AsDateTime > this.Request.Headers.IfModifiedSince);
					if (users != null && users.Any()) {
						users.OrderBy(x => x.LastModified);
						response = this.Request.CreateResponse(HttpStatusCode.OK);
						response.Headers.Add("Last-Modified", users.First().LastModified.AsDateTime.ToString());
					} else response = this.Request.CreateResponse(HttpStatusCode.NotModified);
				} catch (Exception ex) {
					return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
				}
			}

			response.Headers.Add("Type", typeof(UserModel[]).Name);
			response.Headers.Add("Link", GetTaskHeaderLink(this.Request, response));
			return response;
		}

		public HttpResponseMessage Head(string id)
		{
			HttpResponseMessage response = null;
			if (this.Request.Headers.IfModifiedSince.HasValue) {
				try {
					var user = _userRepository.Get(ObjectId.Parse(id));
					if (user.LastModified.AsDateTime > this.Request.Headers.IfModifiedSince.Value) {
						response = this.Request.CreateResponse(HttpStatusCode.OK);
						response.Headers.Add("Last-Modified", user.LastModified.AsDateTime.ToString());
					}
				} catch (Exception ex) {
					return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
				}
			}

			if (response == null) response = this.Request.CreateResponse(HttpStatusCode.NotModified);
			response.Headers.Add("Type", typeof(UserModel).Name);
			response.Headers.Add("Link", GetTaskHeaderLink(this.Request, response));
			return response;
		}

		public HttpResponseMessage Get()
		{
			UserModel[] users = null;

			try {
				users = Mapper.Map<UserModel[]>(_userRepository.GetAll().ToArray());
			} catch (Exception ex) {
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK, users);
			response.Headers.Add("Type", typeof(UserModel[]).Name);
			return response;
		}

		public HttpResponseMessage Get(string id)
		{
			UserModel user = null;

			try {
				user = Mapper.Map<UserModel>(_userRepository.Get(ObjectId.Parse(id)));
			} catch (Exception ex) {
				return this.Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK, user);
			response.Headers.Add("Link", GetTaskHeaderLink(this.Request, response));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Put(UserModel user)
		{
			try {
				var savedUser = _userRepository.Get(ObjectId.Parse(user.Id));
				savedUser.SetName(user.Name);
				_userRepository.Update(savedUser);
			} catch (NullReferenceException ex) {
				return this.Request.CreateResponse(HttpStatusCode.Gone, ex.Message);
			} catch (Exception ex) {
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Link", GetTaskHeaderLink(this.Request, response));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Post(UserModel user)
		{
			var newUser = _userFactory.CreateUser(user.Name);

			try {
				newUser = _userRepository.Add(newUser);
			} catch (Exception ex) {
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.Created);
			response.Headers.Location = new Uri(string.Format("{0}/{1}", this.Request.RequestUri.AbsoluteUri, newUser.Id));
			response.Headers.Add("Link", GetTaskHeaderLink(this.Request, response, newUser.Id.ToString()));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Patch(string id, ExpandoObject user)
		{
			try {
				string newName = null;
				if (user.HasProperty("Name")) newName = ((dynamic)user).Name;

				if (newName != null) {
					var savedUser = _userRepository.Get(ObjectId.Parse(id));
					savedUser.SetName(newName);
					_userRepository.Update(savedUser); 
				}
			} catch (NullReferenceException ex) {
				return this.Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
			} catch (Exception ex) {
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			var response = this.Request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Link", GetTaskHeaderLink(this.Request, response));
			response.Headers.Add("Type", typeof(UserModel).Name);
			return response;
		}

		public HttpResponseMessage Delete(string id)
		{
			try {
				_userRepository.Delete(ObjectId.Parse(id));
			} catch (NullReferenceException ex) {
				return this.Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
			} catch (Exception ex) {
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			return this.Request.CreateResponse(HttpStatusCode.OK);
		}
		
		private string GetTaskHeaderLink(HttpRequestMessage request, HttpResponseMessage response)
		{
			return GetTaskHeaderLink(request, response, string.Empty);
		}

		private string GetTaskHeaderLink(HttpRequestMessage request, HttpResponseMessage response, string id)
		{
			var currentUri = this.Request.RequestUri.AbsoluteUri;
			if (!string.IsNullOrEmpty(id)) currentUri = string.Concat(currentUri, "/", id);

			var uri = new Uri(string.Concat(currentUri, "/tasks"));
			var rel = typeof(TaskModel[]).Name;

			string type = string.Empty;
			if (response != null
				&& response.Content != null
				&& response.Content.Headers != null
				&& response.Content.Headers.ContentType != null
			)
				type = response.Content.Headers.ContentType.MediaType;

			return string.Format("<{0}>; rel={1}; type=\"{2}\"", uri, rel, type);
		}
	}
}
