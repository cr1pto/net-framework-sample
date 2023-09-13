using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System;
using System.Web.Http;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http.Dependencies;
using TimeCards.Lib;
using System.Web.Services.Description;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(TimeCards.Api.Startup))]
namespace TimeCards.Api
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            ConfigureServices(serviceCollection);
            var resolver = new DefaultDependencyResolver(serviceCollection.BuildServiceProvider());
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            DependencyResolver.SetResolver(resolver);
            ConfigureWebApi(app);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
            //services.AddSingleton<IMyInterface, MyClass>();
            services.AddTransient<ITimeTrackingService, TimeTrackingService>();
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            app.UseWebApi(GlobalConfiguration.Configuration);
        }
    }

    public class DefaultDependencyResolver : System.Web.Mvc.IDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver, IDisposable, IAsyncDisposable
    {
        protected IServiceProvider serviceProvider;
        private IServiceScope serviceScope;
        private AsyncServiceScope asyncServiceScope;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IDependencyScope BeginScope()
        {
            serviceScope = serviceProvider.CreateScope();
            asyncServiceScope = serviceProvider.CreateAsyncScope();
            return new DefaultDependencyResolver(asyncServiceScope.ServiceProvider);
        }

        public void Dispose()
        {
            if (serviceScope == null) return;
            serviceScope.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return asyncServiceScope.DisposeAsync();
        }

        public object GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.serviceProvider.GetServices(serviceType);
        }
    }

    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
            IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }
}