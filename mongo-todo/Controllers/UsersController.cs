using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using mongo_todo.Models;

namespace mongo_todo.Controllers
{
    public class UsersController : ApiController
    {
		public IEnumerable<UserModel> Get()
		{
			throw new NotImplementedException();
		}

		public Object Get(int id)
		{
			throw new NotImplementedException();
		}

		public HttpResponseMessage Put(UserModel user)
		{
			throw new NotImplementedException();
		}

		public Object Post(UserModel user)
		{
			throw new NotImplementedException();
		}

		public HttpResponseMessage Delete(UserModel user)
		{
			throw new NotImplementedException();
		}
    }
}
