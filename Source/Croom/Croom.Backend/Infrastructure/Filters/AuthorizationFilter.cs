using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using Croom.Authentication;
using Recognos.Core;

namespace Croom.Backend.Infrastructure.Filters
{
    public class AuthorizationFilter : IAutofacAuthorizationFilter
    {
        private readonly string authTokenHeaderName = "X-Croom-AuthToken";
        private readonly IAuthenticate authenticator;

        public AuthorizationFilter(IAuthenticate authenticator)
        {
            Check.NotNull(authenticator, "authenticator");
            this.authenticator = authenticator;
            Check.InjectedMembers(this);
        }

        public void OnAuthorization(HttpActionContext context)
        {
            Guid token = context.Request.Headers
                .Where(h => h.Key == authTokenHeaderName)
                .Select(h => Guid.Parse(h.Value.Single()))
                .SingleOrDefault();
            if (!IsValidToken(token))
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                return;
            }
        }

        private bool IsValidToken(Guid token)
        {
            if (token == Guid.Empty)
            {
                return false;
            }

            return true;
        }
    }
}