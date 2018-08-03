using System.Web.Http;
using System.Web.OData.Extensions;

namespace ODataSingletonSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.MapODataServiceRoute("odata", "odata", SingletonEdmModel.GetEdmModel());
        }
    }
}
