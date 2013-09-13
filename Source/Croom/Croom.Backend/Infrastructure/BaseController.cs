using Croom.Data;
using Croom.Model;
using Recognos.Core;
using System;
using System.Linq;
using System.Web.Http;

namespace Croom.Backend.Infrastructure
{
    public abstract class BaseController : ApiController
    {
        private readonly string authTokenHeaderName = "X-Croom-AuthToken";
        private User currentUser = null;
        private readonly IStoreDataAsKeyValue store;

        public BaseController(IStoreDataAsKeyValue store)
        {
            Check.NotNull(store, "store");
            this.store = store;
            Check.InjectedMembers(this);
        }

        protected User CurrentUser
        {
            get
            {
                if (currentUser != null)
                {
                    return currentUser;
                }

                Guid token = Request.Headers
                    .Where(h => h.Key == authTokenHeaderName)
                    .Select(h => Guid.Parse(h.Value.Single()))
                    .SingleOrDefault();

                if (token == Guid.Empty)
                {
                    return null;
                }

                currentUser = store.Load(token) as User;

                return currentUser;
            }
        }
    }
}
