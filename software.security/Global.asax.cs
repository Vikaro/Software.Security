using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using Software.Security.App_Start;
using Software.Security.Database;
using Software.Security.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Software.Security
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // Create the container as usual.
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:
            container.Register<ISoftwareSecurityDatabase, SoftwareSecurityDatabase>(lifestyle: Lifestyle.Singleton );
            container.Register<IAuthorizationRepository, AuthorizationRepository>();
            container.Register<IMessageRepository, MessageRepository>();

            // Automapper
            container.RegisterSingleton(() => GetMapper(container));

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private AutoMapper.IMapper GetMapper(Container container)
        {
            var mp = container.GetInstance<AutoMapperConfig>();
            return mp.GetMapper();
        }
    }
  

}
