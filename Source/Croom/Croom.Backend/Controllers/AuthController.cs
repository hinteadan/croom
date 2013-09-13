using Croom.Authentication;
using Croom.Backend.Commands;
using Recognos.Core;
using System;
using System.Linq;
using System.Web.Http;

namespace Croom.Backend.Controllers
{
    public class AuthController : ApiController
    {
        private readonly string authTokenHeaderName = "X-Croom-AuthToken";
        private readonly IAuthenticateLikeNothing authenticator;

        public AuthController(IAuthenticateLikeNothing authenticator)
        {
            Check.NotNull(authenticator, "authenticator");
            this.authenticator = authenticator;
            Check.InjectedMembers(this);
        }

        public LoginResult Put()
        {
            Guid? authToken = null;
            if (!authenticator.Principal(User, out authToken))
            {
                return LoginResult.UnSuccessful();
            }

            return LoginResult.Successful(authToken.Value);
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
            Guid token = Request.Headers
                .Where(h => h.Key == authTokenHeaderName)
                .Select(h => Guid.Parse(h.Value.Single()))
                .SingleOrDefault();

            authenticator.Invalidate(token);
        }
    }
}
