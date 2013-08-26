using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Croom.Data;
using Croom.Model;
using Recognos.Core;

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
                currentUser = store.Load(token) as User;

                return currentUser; 
            }
        }
    }
}
