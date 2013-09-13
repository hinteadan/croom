using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Croom.Data;
using Croom.Model;
using Recognos.Core;

namespace Croom.Authentication.Authenticators
{
    public class DummyAuthenticator : IAuthenticateLikeNothing
    {
        private readonly IStoreDataAsKeyValue store;
        private readonly string[] validUsernames = new string[] { 
            "h"
        };
        private readonly string[] validPasswords = new string[]{
            "123", "p"
        };


        public DummyAuthenticator(IStoreDataAsKeyValue store)
        {
            Check.NotNull(store, "store");
            this.store = store;
            Check.InjectedMembers(this);
        }

        public bool Credentials(string username, string password, out Guid? token)
        {
            token = null;
            if (!validUsernames.CaseInsensitiveContains(username) || !validPasswords.Contains(password))
            {
                return false;
            }

            token = Guid.NewGuid();
            store.SaveOrUpdate(new KeyValuePair<Guid, object>(
                token.Value, 
                new User(username, username, "dan.hintea@recognos.ro"))
                );
            return true;
        }

        public bool Principal(IPrincipal user, out Guid? token)
        {
            Check.NotNull(user, "user");

            token = null;
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }
            token = Guid.NewGuid();
            store.SaveOrUpdate(new KeyValuePair<Guid, object>(token.Value, user.Identity.ToUser()));
            return true;
        }

        public bool Authenticate(Guid token, out User authenticatedUser)
        {
            authenticatedUser = store.Load(token) as User;
            return authenticatedUser != null;
        }

        public bool Authenticate(Guid token)
        {
            User user;
            return this.Authenticate(token, out user);
        }

        public void Invalidate(Guid token)
        {
            store.Remove(token);
        }
    }
}
