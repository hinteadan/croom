using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Croom.Authentication;
using Croom.Authentication.Authenticators;
using Croom.Backend.Controllers;
using Croom.Backend.Infrastructure.Filters;
using Croom.Data.Stores;

namespace Croom.Backend.Infrastructure
{
    internal class Dependencies
    {
        internal static void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            builder.RegisterInstance<InMemoryStore>(new InMemoryStore()).AsImplementedInterfaces();
            builder.RegisterType<DummyAuthenticator>().AsImplementedInterfaces();

            builder.RegisterType<AuthorizationFilter>()
                .AsWebApiAuthorizationFilterFor<ReservationController>()
                .InstancePerApiRequest();

            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}