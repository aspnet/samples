using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace WebApiAttributeRoutingSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.XmlFormatter.AddUriPathExtensionMapping("xml", XmlMediaTypeFormatter.DefaultMediaType);
            config.Formatters.JsonFormatter.AddUriPathExtensionMapping("json", JsonMediaTypeFormatter.DefaultMediaType);
        

            // Web API routes

            // NOTE:
            // As per general routing guidelines more specific routes should be registered before generic routes.
            // Since attribute routing creates more specific routes, make sure to register it before any conventional routes.
            // 
            // Example: 
            // if the below registration order was reversed, then requests for attributed controllers would be matched by
            // the 'DefaultApi' route and would result in a 404 response. Conventional routes can never access attribute based
            // controller/actions.

            // attribute routes
            config.MapHttpAttributeRoutes();

            // conventional routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
