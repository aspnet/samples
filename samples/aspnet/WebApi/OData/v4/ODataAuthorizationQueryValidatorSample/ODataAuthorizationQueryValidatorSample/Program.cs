using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using ODataAuthorizationQueryValidatorSample.Extensions;
using ODataAuthorizationQueryValidatorSample.Model;
using ODataAuthorizationQueryValidatorSample.SampleHelpers;
using Owin;
using System;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ODataAuthorizationQueryValidatorSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceUrl = "http://localhost:12345";
            using (WebApp.Start(serviceUrl, Configuration))
            {
                Console.WriteLine("Listening on {0}", serviceUrl);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        public static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration configuration = new HttpConfiguration();

            // We add a fake authentication attribute to demonstrate the various cases.
            configuration.Filters.Add(new FakeAuthenticationAttribute());
 
            configuration.MapODataServiceRoute("odata", "odata", GetModel());
            configuration.Routes.MapHttpRoute("api", "api/{controller}");

            builder.UseWebApi(configuration);
        }

        public static IEdmModel GetModel()
        {
            // Define the model.
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Address>("Addresses");
            builder.EntitySet<OrderLine>("OrdersLines");

            // Build the model and add the authorized roles annotation that will be consumed by the 
            // Authorization query validator.
            IEdmModel model = builder.GetEdmModel();

            // Inspect the CLR types associated with this model for CanExpandAttribute instances and
            // add AuthorizedRolesAnnotation instances as appropiate.
            model.AddAuthorizedRolesAnnotations();
            return model;
        }
    }
}
