using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Croom.Data;
using Croom.Model;
using Recognos.Core;

namespace Croom.Authentication.Authenticators
{
    public class ActiveDirectoryAuthenticator : IAuthenticateLikeNothing
    {
        private readonly IStoreDataAsKeyValue store;
        private readonly string domain = "thor.recognos.ro";
        private readonly string domainConnectionString = "LDAP://thor.recognos.ro";

        public ActiveDirectoryAuthenticator(IStoreDataAsKeyValue store)
        {
            Check.NotNull(store, "store");
            this.store = store;
            string configDomain = ConfigurationManager.AppSettings["ActiveDirectory.Domain"];
            if (!string.IsNullOrWhiteSpace(configDomain))
            {
                this.domain = configDomain;
                this.domainConnectionString = string.Format("LDAP://{0}", configDomain);
            }
            Check.InjectedMembers(this);
        }

        public bool Credentials(string username, string password, out Guid? token)
        {
            token = null;
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                bool isValid = context.ValidateCredentials(username, password);
                if (isValid)
                {
                    token = Guid.NewGuid();
                    using (var directory = new DirectorySearcher(domainConnectionString))
                    {
                        directory.Filter = string.Format("(sAMAccountName={0})", username);
                        DirectoryEntry entry = directory.FindOne().GetDirectoryEntry();
                        store.SaveOrUpdate(new KeyValuePair<Guid, object>(token.Value, entry.ToUser()));
                    }
                }
                return isValid;
            }
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
