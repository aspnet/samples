using System.Web.Http;

namespace CustomMediaTypeFormatter
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register the custom media type formatter. In this case
            // we just add it to the list of default formatters. We could
            // also make it the only formatter by clearing out the
            // Formatters collection first.
            config.Formatters.Add(new PlainTextBufferedFormatter());
        }
    }
}
