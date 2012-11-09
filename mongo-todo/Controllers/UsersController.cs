using AutoMapper;
using Domain;
using mongo_todo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace mongo_todo.Controllers
{
    public class UsersController : ApiController
    {
		private readonly IUserRepository _userRepository;
		public UsersController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[ActionName("all")]
		public IEnumerable<UserModel> Get()
		{
			return Mapper.Map<IEnumerable<UserModel>>(_userRepository.GetAll().ToList());
		}

		[ActionName("user")]
		public Object Get(string id)
		{
			return Mapper.Map<UserModel>(_userRepository.Get(ObjectId.Parse(id)));
		}

		[ActionName("user")]
		public HttpResponseMessage Put(UserModel user)
		{
			throw new NotImplementedException();
		}

		[ActionName("user")]
		public UserModel Post(UserModel user)
		{
			throw new NotImplementedException();
		}

		[ActionName("user")]
		public HttpResponseMessage Delete(UserModel user)
		{
			throw new NotImplementedException();
		}
    }
}
