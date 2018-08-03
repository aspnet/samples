using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using FunctionSample.Models;
using Microsoft.OData.Edm;
using Owin;

namespace FunctionSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            var config = new HttpConfiguration();
            config.MapODataServiceRoute(routeName: "odata route", routePrefix: "odata", model: GetEdmMode());
            builder.UseWebApi(config);
        }

        private IEdmModel GetEdmMode()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<Product>("Products");

            var productType = builder.EntityType<Product>();

            // Function bound to a collection
            // Returns the most expensive product, a single entity
            productType.Collection
                .Function("MostExpensive")
                .Returns<double>();

            // Function bound to a collection
            // Returns the top 10 product, a collection
            productType.Collection
                .Function("Top10")
                .ReturnsCollectionFromEntitySet<Product>("Products");

            // Function bound to a single entity
            // Returns the instance's price rank among all products
            productType
                .Function("GetPriceRank")
                .Returns<int>();

            // Function bound to a single entity
            // Accept a string as parameter and return a double
            // This function calculate the general sales tax base on the 
            // state
            productType
                .Function("CalculateGeneralSalesTax")
                .Returns<double>()
                .Parameter<string>("state");

            // Unbound Function
            builder.Function("GetSalesTaxRate")
                .Returns<double>()
                .Parameter<string>("state");

            return builder.GetEdmModel();
        }
    }
}