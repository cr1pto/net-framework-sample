using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace TimeCards.Web
{
    public static class WebApiConfig
    {
        private static JsonSerializerSettings CreateJsonSerializer() => new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.Objects,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services           

            //config.Formatters.Add(new XmlMediaTypeFormatter
            //{
            //    UseXmlSerializer = true,
            //    WriterSettings = new System.Xml.XmlWriterSettings
            //    {

            //    }

            //});
            //config.Formatters.Add(config.Formatters.XmlFormatter);

            //config.Formatters.XmlFormatter.UseXmlSerializer = false;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = CreateJsonSerializer();
            JsonConvert.DefaultSettings = () => GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );
        }
    }
}
