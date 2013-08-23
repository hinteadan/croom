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
using Croom.Data.Stores;

namespace Croom.Backend.Infrastructure
{
    internal class Dependencies
    {
        internal static void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterInstance<InMemoryStore>(new InMemoryStore()).AsImplementedInterfaces();
            builder.RegisterType<ActiveDirectoryAuthenticator>().AsImplementedInterfaces();

            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(builder.Build());
        }
    }
}