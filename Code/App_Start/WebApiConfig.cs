using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BnbApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.JsonFormatter);
            var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            xml.UseXmlSerializer = true;
            config.Formatters.Add(xml);
            //config.Filters.Add(new UnhandledExceptionFilter());
        }
    }
}
