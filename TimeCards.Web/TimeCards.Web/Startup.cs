using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using TimeCards.Lib;

[assembly: OwinStartup(typeof(TimeCards.Web.Startup))]
namespace TimeCards.Web
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = ConfigureDi(app);
            ConfigureWebApi(app, container);
        }

        private IContainer ConfigureDi(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            AddServices(builder);

            return builder.Build();
        }

        private void AddServices(ContainerBuilder builder)
        {
            builder.RegisterType<TimeTrackingService>().As<ITimeTrackingService>();
            //builder.RegisterInstance<ITimeTrackingService>().As<TimeTrackingService>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }

        private void ConfigureWebApi(IAppBuilder app, IContainer container)
        {
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(GlobalConfiguration.Configuration);
            app.UseWebApi(GlobalConfiguration.Configuration);
        }
    }
}