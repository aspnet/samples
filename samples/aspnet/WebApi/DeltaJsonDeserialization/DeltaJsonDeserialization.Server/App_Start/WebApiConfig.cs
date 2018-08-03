using System.Web.Http;

namespace DeltaJsonDeserialization.Server
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();

            var jsonFormatter = configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new DeltaContractResolver(jsonFormatter);
        }
    }
}