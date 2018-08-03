using System.Web.Http;
using System.Web.OData.Extensions;

namespace DynamicEdmModelCreation
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            DynamicModelHelper.CustomMapODataServiceRoute(
                config.Routes,
                "odata",
                "odata");
            config.AddODataQueryFilter();
        }
    }
}
