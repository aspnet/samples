using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ODataQueryableSample
{
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:50231/");

        static void Main(string[] args)
        {
            try
            {
                // Start listening
                using (WebApp.Start<Startup>(url: _baseAddress.OriginalString))
                {
                    Console.WriteLine("Listening on " + _baseAddress);

                    // Run clients against each of the three sample controllers
                    RunCustomerClient();
                    RunOrderClient();
                    RunResponseClient();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
            }
            finally
            {
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
        }

        /// <summary>
        /// This client issues requests against the CustomerController
        /// </summary>
        static void RunCustomerClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _baseAddress;

            // Without any query we get the whole content
            HttpResponseMessage response = client.GetAsync("/odata/customer/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOriginal list returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id
            response = client.GetAsync("/odata/customer/?$orderby=Id").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then skip by 1 and take the first two
            response = client.GetAsync("/odata/customer/?$orderby=Id&$skip=1&$top=2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id, return the second and third one: " + response.Content.ReadAsStringAsync().Result);

            // order by Name
            response = client.GetAsync("/odata/customer/?$orderby=Name").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Name returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Name then skip by 2 and take the first one
            response = client.GetAsync("/odata/customer/?$orderby=Name&$skip=2&$top=1").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Name, return the third one: " + response.Content.ReadAsStringAsync().Result);

            // find customers with at least one order with a quantity greater than or equal to 10
            response = client.GetAsync("/odata/customer/?$filter=Orders/any(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with Any Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);

            // find customers with orders that all have a quantity greater than or equal to 10
            response = client.GetAsync("/odata/customer/?$filter=Orders/all(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with All Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);

            // unsupported operator starts with $- 400
            response = client.GetAsync("/odata/customer/?$orderby=Name&$unknown=12").Result;
            Console.WriteLine("\nOrderBy Name with another $unknown query returned: " + response);

            // unsupported operator not starting with $- ignored
            response = client.GetAsync("/odata/customer/?$orderby=Name&unknown=12").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Name with another unknown query returned: " + response.Content.ReadAsStringAsync().Result);

            // invalid operator - 400
            response = client.GetAsync("/odata/customer/?$orderby=UnknownPropertyName").Result;
            Console.WriteLine("\nOrderBy UnknownPropertyName query returned: " + response);

            // filter by Name
            response = client.GetAsync("/odata/customer/?$filter=Name eq 'Lowest'").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter Name query returned: " + response.Content.ReadAsStringAsync().Result);

            // filter by expression
            response = client.GetAsync("/odata/customer/?$filter=Id add 2 eq 4").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter by expression: " + response.Content.ReadAsStringAsync().Result);

            // filter with string length method call
            response = client.GetAsync("/odata/customer/?$filter=length(Name) eq 6").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with string length call: " + response.Content.ReadAsStringAsync().Result);

            // filter with datetime year method call
            response = client.GetAsync("/odata/customer/?$filter=year(BirthTime) eq 2001").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with year call: " + response.Content.ReadAsStringAsync().Result);

            // filter with expression using multiplication is not allowed
            response = client.GetAsync("/odata/customer/?$filter=Id mul 2 eq 6").Result;
            Console.WriteLine("\nFilter with multiplication in the expression is not allowed: " + response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// This client issues requests against the OrderController
        /// </summary>
        static void RunOrderClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _baseAddress;

            // Without any query we get the whole content
            HttpResponseMessage response = client.GetAsync("/odata/order/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOriginal list returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id
            response = client.GetAsync("/odata/order/?$orderby=Id").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then skip by 1 and take the first two
            response = client.GetAsync("/odata/order/?$orderby=Id&$skip=1&$top=2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id, return the second and third one: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then take the first two thousand. This will result in an error due to our check in the OrderController which only allows Top up to 9.
            response = client.GetAsync("/odata/order/?$orderby=Id&$top=2000").Result;
            Console.WriteLine("\nOrderBy Id with invalid top value: " + response.Content.ReadAsStringAsync().Result);

            // order by Name and then take the first two. This will result in an error due to our check in the OrderController which only allows order by Id property.
            response = client.GetAsync("/odata/order/?$orderby=Name&$top=2").Result;
            Console.WriteLine("\nOrderBy Name is not allowed: " + response.Content.ReadAsStringAsync().Result);

            // Filter the orders to return those whose Id is bigger than 10. 
            response = client.GetAsync("/odata/order/?$filter=Id ge 10").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with Id, returns 3 orders: " + response.Content.ReadAsStringAsync().Result);

            // Filter the orders to return those whose quantity is bigger or equal to 100. This will result in an error due to our check in the 
            // OrderController which disallows filtering orders based on its quantity.
            response = client.GetAsync("/odata/order/?$filter=Quantity ge 100").Result;
            Console.WriteLine("\nFilter with Quantity is not allowed: " + response.Content.ReadAsStringAsync().Result);

            // select name and birth time
            response = client.GetAsync("/odata/customer/?$select=Name,BirthTime").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nSelect a subset of the fields of an entity: " + response.Content.ReadAsStringAsync().Result);

            // expand the orders of a customer
            response = client.GetAsync("/odata/customer/?$expand=Orders").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nExpand the orders of a customer: " + response.Content.ReadAsStringAsync().Result);

            // select and expand combined
            response = client.GetAsync("/odata/customer/?$expand=Orders&$select=Name,Orders/Name,Orders/Quantity").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nSelect and expand combined: " + response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// This client issues requests against the ResponseController which returns an HttpResponseMessage
        /// containing a custom HTTP header field.
        /// </summary>
        static void RunResponseClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _baseAddress;

            // Without any query we get the whole content
            HttpResponseMessage response = client.GetAsync("/odata/response/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nOriginal list returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id
            response = client.GetAsync("/odata/response/?$orderby=Id").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nOrderBy Id returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then skip by 1 and take the first two
            response = client.GetAsync("/odata/response/?$orderby=Id&$skip=1&$top=2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nOrderBy Id, return the second and third one: " + response.Content.ReadAsStringAsync().Result);

            // find customers with at least one order with a quantity greater than or equal to 10
            response = client.GetAsync("/odata/response/?$filter=Orders/any(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nFilter with Any Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);

            // find customers with orders that all have a quantity greater than or equal to 10
            response = client.GetAsync("/odata/response/?$filter=Orders/all(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nFilter with All Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);
        }
    }
}
