using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Croom.Backend.Infrastructure;
using Croom.Model;

namespace Croom.Backend.Controllers
{
    public class UserController : BaseController
    {
        public User Get()
        {
            return CurrentUser;
        }
    }
}
