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
	public class UsersController :ApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IUserFactory _userFactory;
		public UsersController(IUserRepository userRepository, IUserFactory userFactory)
		{
			_userRepository = userRepository;
			_userFactory = userFactory;
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
			response.Headers.Add("Type", users.GetType().Name);
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
			response.Headers.Add("Link", string.Format("{0}/tasks", this.Request.RequestUri.AbsoluteUri));
			response.Headers.Add("Type", user.GetType().Name);
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
			response.Headers.Add("Link", string.Format("{0}/tasks", response.Headers.Location));
			response.Headers.Add("Type", newUser.GetType().Name);
			return response;
		}

		public HttpResponseMessage Delete(UserModel user)
		{
			try {
				_userRepository.Delete(ObjectId.Parse(user.Id));
			} catch (NullReferenceException ex) {
				return this.Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
			} catch (Exception ex) {
				return this.Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			return this.Request.CreateResponse(HttpStatusCode.OK);
		}
	}
}
