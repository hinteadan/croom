using Croom.Model;
using System;
using System.Security.Principal;

namespace Croom.Authentication
{
    public interface IAuthenticate
    {
        bool Credentials(string username, string password, out Guid? token);
        bool Principal(IPrincipal user, out Guid? token);
        bool Authenticate(Guid token, out User authenticatedUser);
        bool Authenticate(Guid token);
        void Invalidate(Guid token);
    }

    public interface IAuthenticateLikeNothing : IAuthenticate { }
}
