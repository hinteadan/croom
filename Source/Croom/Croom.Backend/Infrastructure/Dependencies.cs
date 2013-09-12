using Autofac;
using Autofac.Integration.WebApi;
using Croom.Authentication.Authenticators;
using Croom.Backend.Controllers;
using Croom.Backend.Infrastructure.Filters;
using Croom.Data.Stores;
using Croom.Model;
using System;
using System.Reflection;
using System.Web.Http;

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
            //builder.RegisterType<ActiveDirectoryAuthenticator>().AsImplementedInterfaces();

            builder.RegisterType<AuthorizationFilter>()
                .AsWebApiAuthorizationFilterFor<ReservationController>(c => c.Post(default(Reservation)))
                .InstancePerApiRequest();
            builder.RegisterType<AuthorizationFilter>()
                .AsWebApiAuthorizationFilterFor<ReservationController>(c => c.Put(default(Guid), default(Reservation)))
                .InstancePerApiRequest();
            builder.RegisterType<AuthorizationFilter>()
                .AsWebApiAuthorizationFilterFor<ReservationController>(c => c.Delete(default(Guid)))
                .InstancePerApiRequest();

            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}