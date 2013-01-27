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
	public class UsersController :ApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IUserFactory _userFactory;
		public UsersController(IUserRepository userRepository, IUserFactory userFactory)
		{
			_userRepository = userRepository;
			_userFactory = userFactory;
		}

		public IEnumerable<UserModel> Get()
		{
			return Mapper.Map<IEnumerable<UserModel>>(_userRepository.GetAll().ToList());
		}

		public UserModel Get(string id)
		{
			return Mapper.Map<UserModel>(_userRepository.Get(ObjectId.Parse(id)));
		}

		public HttpResponseMessage Put(UserModel user)
		{
			try {
				var savedUser = _userRepository.Get(ObjectId.Parse(user.Id));
				savedUser.SetName(user.Name);
				_userRepository.Update(savedUser);
			} catch (NullReferenceException) {
				return new HttpResponseMessage(HttpStatusCode.Gone);
			} catch {
				return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		public UserModel Post(UserModel user)
		{
			var newUser = _userFactory.CreateUser(user.Name);
			newUser = _userRepository.Add(newUser);

			return Mapper.Map<UserModel>(newUser);
		}

		public HttpResponseMessage Delete(UserModel user)
		{
			try {
				_userRepository.Delete(ObjectId.Parse(user.Id));
			} catch (NullReferenceException) {
				return new HttpResponseMessage(HttpStatusCode.Gone);
			} catch {
				return new HttpResponseMessage(HttpStatusCode.InternalServerError);
			}
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
