using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Croom.Authentication;
using Croom.Backend.Commands;
using Recognos.Core;

namespace Croom.Backend.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IAuthenticateLikeNothing authenticator;

        public AuthController(IAuthenticateLikeNothing authenticator)
        {
            Check.NotNull(authenticator, "authenticator");
            this.authenticator = authenticator;
            Check.InjectedMembers(this);
        }

        public LoginResult Post(LoginCommand credentials)
        {
            Guid? authToken = null;
            if (!authenticator.Credentials(credentials.Username, credentials.Password, out authToken))
            {
                return LoginResult.UnSuccessful();
            }

            return LoginResult.Successful(authToken.Value);
        }

        public void Delete()
        {

        }
    }
}
