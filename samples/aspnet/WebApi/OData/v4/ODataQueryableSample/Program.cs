using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

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

                    // Run clients against each of controllers
                    RunCustomerClient();
                    RunOrderClient();
                    RunResponseClient();
                    RunEmployeeClient();
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
        /// This client issues requests against the CustomersController
        /// </summary>
        static void RunCustomerClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _baseAddress;

            // Without any query we get the whole content
            HttpResponseMessage response = client.GetAsync("/odata/Customers/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOriginal list returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id
            response = client.GetAsync("/odata/Customers/?$orderby=Id").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then skip by 1 and take the first two
            response = client.GetAsync("/odata/Customers/?$orderby=Id&$skip=1&$top=2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id, return the second and third one: " + response.Content.ReadAsStringAsync().Result);

            // order by Name
            response = client.GetAsync("/odata/Customers/?$orderby=Name").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Name returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Name then skip by 2 and take the first one
            response = client.GetAsync("/odata/Customers/?$orderby=Name&$skip=2&$top=1").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Name, return the third one: " + response.Content.ReadAsStringAsync().Result);

            // find customers with at least one order with a quantity greater than or equal to 10
            response = client.GetAsync("/odata/Customers/?$filter=Orders/any(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with Any Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);

            // find customers with orders that all have a quantity greater than or equal to 10
            response = client.GetAsync("/odata/Customers/?$filter=Orders/all(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with All Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);

            // unsupported operator starts with $- 400
            response = client.GetAsync("/odata/Customers/?$orderby=Name&$unknown=12").Result;
            Console.WriteLine("\nOrderBy Name with another $unknown query returned: " + response);

            // unsupported operator not starting with $- ignored
            response = client.GetAsync("/odata/Customers/?$orderby=Name&unknown=12").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Name with another unknown query returned: " + response.Content.ReadAsStringAsync().Result);

            // invalid operator - 400
            response = client.GetAsync("/odata/Customers/?$orderby=UnknownPropertyName").Result;
            Console.WriteLine("\nOrderBy UnknownPropertyName query returned: " + response);

            // filter by Name
            response = client.GetAsync("/odata/Customers/?$filter=Name eq 'Lowest'").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter Name query returned: " + response.Content.ReadAsStringAsync().Result);

            // filter by expression
            response = client.GetAsync("/odata/Customers/?$filter=Id add 2 eq 4").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter by expression: " + response.Content.ReadAsStringAsync().Result);

            // filter with string length method call
            response = client.GetAsync("/odata/Customers/?$filter=length(Name) eq 6").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with string length call: " + response.Content.ReadAsStringAsync().Result);

            // filter with datetime year method call
            response = client.GetAsync("/odata/Customers/?$filter=year(BirthTime) eq 2001").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with year call: " + response.Content.ReadAsStringAsync().Result);

            // filter by expression using parameter alias
            response = client.GetAsync("/odata/Customers/?$filter=@p1&@p1=year(BirthTime) eq 2001").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nfilter by expression using parameter alias: " + response.Content.ReadAsStringAsync().Result);

            // filter with expression using multiplication is not allowed
            response = client.GetAsync("/odata/Customers/?$filter=Id mul 2 eq 6").Result;
            Console.WriteLine("\nFilter with multiplication in the expression is not allowed: " + response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// This client issues requests against the OrdersController
        /// </summary>
        static void RunOrderClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _baseAddress;

            // Without any query we get the whole content
            HttpResponseMessage response = client.GetAsync("/odata/Orders/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOriginal list returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id
            response = client.GetAsync("/odata/Orders/?$orderby=Id").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then skip by 1 and take the first two
            response = client.GetAsync("/odata/Orders/?$orderby=Id&$skip=1&$top=2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nOrderBy Id, return the second and third one: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then take the first two thousand. This will result in an error due to our check in the OrdersController which only allows Top up to 9.
            response = client.GetAsync("/odata/Orders/?$orderby=Id&$top=2000").Result;
            Console.WriteLine("\nOrderBy Id with invalid top value: " + response.Content.ReadAsStringAsync().Result);

            // order by Name and then take the first two. This will result in an error due to our check in the OrdersController which only allows order by Id property.
            response = client.GetAsync("/odata/Orders/?$orderby=Name&$top=2").Result;
            Console.WriteLine("\nOrderBy Name is not allowed: " + response.Content.ReadAsStringAsync().Result);

            // Filter the orders to return those whose Id is bigger than 10. 
            response = client.GetAsync("/odata/Orders/?$filter=Id ge 10").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter with Id, returns 3 orders: " + response.Content.ReadAsStringAsync().Result);

            // Filter the orders to return those whose quantity is bigger or equal to 100. This will result in an error due to our check in the 
            // OrdersController which disallows filtering orders based on its quantity.
            response = client.GetAsync("/odata/Orders/?$filter=Quantity ge 100").Result;
            Console.WriteLine("\nFilter with Quantity is not allowed: " + response.Content.ReadAsStringAsync().Result);

            // select name and birth time
            response = client.GetAsync("/odata/Customers/?$select=Name,BirthTime").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nSelect a subset of the fields of an entity: " + response.Content.ReadAsStringAsync().Result);

            // expand the orders of a customer
            response = client.GetAsync("/odata/Customers/?$expand=Orders").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nExpand the orders of a customer: " + response.Content.ReadAsStringAsync().Result);

            // select and expand combined
            response = client.GetAsync("/odata/Customers/?$select=Name&$expand=Orders($select=Name,Quantity)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nSelect and expand combined: " + response.Content.ReadAsStringAsync().Result);

            // filter and orderby using parameter alias
            response = client.GetAsync(
                    "/odata/Customers/?$filter=Gender eq @p1&$expand=Orders($orderby=@p2)&@p1=ODataQueryableSample.Models.Gender'Female'&@p2=Origin/City").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter and orderby using parameter alias: " + response.Content.ReadAsStringAsync().Result);

            // filter using unspecified parameter alias
            response = client.GetAsync(
                    "/odata/Customers/?$filter=Gender eq @p1").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nFilter using unspecified parameter alias: " + response.Content.ReadAsStringAsync().Result);

            // delete an order of a customer by specifying relative $id
            response = client.DeleteAsync("/odata/Customers(11)/Orders/$ref?$id=../../Orders(0)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nDelete an order of a customer: " + response.Content.ReadAsStringAsync().Result);

            // delete an order of a customer by specifying absolute $id
            response = client.DeleteAsync(string.Format("/odata/Customers(11)/Orders/$ref?$id={0}odata/Orders(1)", client.BaseAddress)).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nDelete an order of a customer: " + response.Content.ReadAsStringAsync().Result);
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
            HttpResponseMessage response = client.GetAsync("/odata/Response/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nOriginal list returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id
            response = client.GetAsync("/odata/Response/?$orderby=Id").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nOrderBy Id returned: " + response.Content.ReadAsStringAsync().Result);

            // order by Id and then skip by 1 and take the first two
            response = client.GetAsync("/odata/Response/?$orderby=Id&$skip=1&$top=2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nOrderBy Id, return the second and third one: " + response.Content.ReadAsStringAsync().Result);

            // find customers with at least one order with a quantity greater than or equal to 10
            response = client.GetAsync("/odata/Response/?$filter=Orders/any(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nFilter with Any Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);

            // find customers with orders that all have a quantity greater than or equal to 10
            response = client.GetAsync("/odata/Response/?$filter=Orders/all(order: order/Quantity ge 10)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nFilter with All Order/Quantity ge 10: " + response.Content.ReadAsStringAsync().Result);

            // delete order from customer
            response = client.DeleteAsync("/odata/Response(11)/Orders/$ref?$id=../../Orders(0)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nHTTP response headers:\n{0}", response.Headers);
            Console.WriteLine("\nDelete order from customer: " + response.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// $levels request against EmployeesController
        /// </summary>
        static void RunEmployeeClient() 
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = _baseAddress;

            // Without any query
            HttpResponseMessage response = client.GetAsync("/odata/Employees/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\nWithout any query, returned:\n" + response.Content.ReadAsStringAsync().Result);

            // Expand Manager to 3 levels of recursion
            response = client.GetAsync("/odata/Employees?$expand=Manager($levels=3)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\n$expand=Manager($levels=3), returned:\n" + response.Content.ReadAsStringAsync().Result);

            // Expand with $select
            response = client.GetAsync("/odata/Employees?$expand=Manager($levels=3; $select=Name,ID)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\n$expand=Manager($levels=3; $select=Name,ID), returned:\n" + response.Content.ReadAsStringAsync().Result);

            // Expand multiple navigation properties 
            response = client.GetAsync("/odata/Employees?$expand=Manager($levels=3), Friend($levels=5)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\n$expand=Manager($levels=3), Friend($levels=5), returned:\n" + response.Content.ReadAsStringAsync().Result);

            // Expand derived type navigation property
            response = client.GetAsync("/odata/Employees?$expand=ODataQueryableSample.Models.Manager/DirectReports($levels=max)").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\n$expand=ODataQueryableSample.Models.Manager/DirectReports($levels=max), returned:\n" + response.Content.ReadAsStringAsync().Result);

            // Expand with nested $expand
            response = client.GetAsync("/odata/Employees/ODataQueryableSample.Models.Manager?$expand=DirectReports($levels=3;$expand=Manager($levels=2))").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("\n$expand=DirectReports($levels=3;$expand=Manager($levels=2)), returned:\n" + response.Content.ReadAsStringAsync().Result);
        }
    }
}
