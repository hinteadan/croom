using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Croom.Model;

namespace Croom.Backend.Infrastructure
{
    public abstract class BaseController : ApiController
    {
        private readonly User currentUser = new User("dummy", "Dummy User", "aaa@aaa.com");

        protected User CurrentUser
        {
            get
            {
                return currentUser; 
            }
        }
    }
}
