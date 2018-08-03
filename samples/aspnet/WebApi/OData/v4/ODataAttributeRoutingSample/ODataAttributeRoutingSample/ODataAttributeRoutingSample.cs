using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using ODataAttributeRoutingSample.Models;
using Owin;

namespace ODataAttributeRoutingSample
{
    public class ODataAttributeRoutingSample
    {
        private static HttpClient client = new HttpClient();
        private const string ServiceUrl = "http://localhost:12345";

        public static void Main(string[] args)
        {
            using (WebApp.Start(ServiceUrl, Configuration))
            {
                Console.WriteLine("Server is listening at {0}", ServiceUrl);

                RunSample();

                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey();
            }
        }

        public static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            configuration.MapODataServiceRoute("odata", null, GetEdmModel());
            builder.UseWebApi(configuration);
        }

        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            EntityTypeConfiguration<Customer> customer = builder.EntitySet<Customer>("Customers").EntityType;
            EntityTypeConfiguration<Order> order = builder.EntitySet<Order>("Orders").EntityType;

            // Add unbound function for get special customer.
            FunctionConfiguration getCustomersWithNameContaining = builder.Function("GetCustomersWithNameContaining");
            getCustomersWithNameContaining.Parameter<string>("ContainedString");
            getCustomersWithNameContaining.ReturnsCollectionFromEntitySet<Customer>("Customers");

            // Add bound action for updating order price.
            ActionConfiguration updateOrderPrice = order.Action("UpdateOrderPrice");
            updateOrderPrice.Parameter<string>("Price");
            updateOrderPrice.ReturnsFromEntitySet<Order>("Orders");

            // Add bound function for delete order from customer and return the rest orders.
            FunctionConfiguration deleteOrderFromCustomer = customer.Function("DeleteOrderFromCustomer");
            deleteOrderFromCustomer.Parameter<int>("OrderId");
            deleteOrderFromCustomer.ReturnsCollectionFromEntitySet<Order>("Orders");

            return builder.GetEdmModel();
        }

        public static void RunSample()
        {
            // Unbound function can only be called with attribute routing.
            Console.WriteLine("1. Call unbound function to get special customers.");
            CallUnboundFunctionToGetSpecialCustomers("2");

            Console.WriteLine("\n2. Call bound action to update order price.");
            CallBoundActionToUpdateOrderPrice(1, 10, 1.23);

            Console.WriteLine("\n3. Add an order to the customer.");
            AddOrderToCustomer(3, 33, 99.99);

            Console.WriteLine("\n4. Call bound function to delete order from customer and return the rest orders.");
            CallBoundFunctionToDeleteOrderFromCustomer(3, 31);

            Console.WriteLine("\n5. Get all customers with attribute routing.");
            GetAllCustomersWithAttriubteRouting();
        }

        public static void CallUnboundFunctionToGetSpecialCustomers(string str)
        {
            HttpResponseMessage response = client.GetAsync(ServiceUrl +
                String.Format("/GetCustomersWithNameContaining(ContainedString='{0}')", str)).Result;

            PrintResponse(response);
        }

        public static void CallBoundActionToUpdateOrderPrice(int customerId, int orderId, double price)
        {
            HttpResponseMessage response = client.PostAsync(ServiceUrl +
                String.Format(
                    "/Customers({0})/Orders({1})/Default.UpdateOrderPrice",
                    customerId, orderId),
                new StringContent(String.Format("{{\"Price\":\"{0}\"}}", price), Encoding.Default, "application/json")).Result;

            PrintResponse(response);
        }

        public static void AddOrderToCustomer(int customerId, int orderId, double price)
        {
            HttpResponseMessage response = client.PostAsync(ServiceUrl +
                String.Format(
                    "/Customers({0})/Orders/$ref",
                    customerId, orderId),
                new StringContent(String.Format("{{\"Id\":{0},\"Price\":\"{1}\"}}", orderId, price), Encoding.Default, "application/json")).Result;

            PrintResponse(response);
        }

        public static void CallBoundFunctionToDeleteOrderFromCustomer(int customerId, int orderId)
        {
            HttpResponseMessage response = client.GetAsync(ServiceUrl +
                String.Format("/Customers({0})/Default.DeleteOrderFromCustomer(OrderId={1})", customerId, orderId)).Result;

            PrintResponse(response);
        }

        public static void GetAllCustomersWithAttriubteRouting()
        {
            HttpResponseMessage response = client.GetAsync(ServiceUrl + "/Customers").Result;
            PrintResponse(response);
        }

        public static void PrintResponse(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Response:");
            Console.WriteLine(response);

            if (response.Content != null)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}