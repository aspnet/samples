using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace ODataAuthorizationQueryValidatorSample.Client
{
    class Program
    {
        private static readonly string odataUrl = "http://localhost:12345/odata/Customers";
        private static readonly string webApiUrl = "http://localhost:12345/api/ApiCustomers";

        static void Main(string[] args)
        {
            HttpResponseMessage response;
            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            // Send an OData request with valid "Authentication credentials" and a restriction configured on the type.
            // The validation will pass as the user is member of two roles, Administrator, that was required to access
            // the Orders entity set, and Inspector, that was defined using the CanExpand attribute.
            response = SendODataRequest(expandClause: "Orders($expand=ShippingAddress)", user: "javier",
                roles: "Inspector, Administrator");
            Console.WriteLine(response.ReasonPhrase);
            Console.WriteLine(GetFirstJsonItem(response));
            Console.WriteLine("---------------------------");
            Console.WriteLine();

            // Send an OData request with invalid "Authentication credentials" and a restriction configured on the type.
            // The validation won't pass as the user only has permissions to access the Orders entity set but isn't allowed
            // to access any navigation source of type Address as it's not a member of the Inspector role.
            response = SendODataRequest(expandClause: "Orders($expand=ShippingAddress)", user: "javier", roles: "Administrator");
            Console.WriteLine(response.ReasonPhrase);
            Console.WriteLine(GetErrorMessage(response));
            Console.WriteLine("---------------------------");
            Console.WriteLine();

            // Send a Web API request with valid "Authentication credentials"
            // For regular web api, we can just use the CanExpand attribute to define the roles applicable to the type.
            // The restriction is defined on the type, but the user is a member of the restricted set of roles.
            response = SendWebApiRequest(expandClause: "Orders($expand=Items)", user: "javier", roles: "Manager");
            Console.WriteLine(response.ReasonPhrase);
            Console.WriteLine(GetFirstJsonItem(response));
            Console.WriteLine("---------------------------");
            Console.WriteLine();

            // Send a Web API request with invalid "Authentication credentials"
            // In this case, the user is a member of a different role, so validation won't pass.
            response = SendWebApiRequest(expandClause: "Orders($expand=Items)", user: "javier", roles: "Administrator");
            Console.WriteLine(response.ReasonPhrase);
            Console.WriteLine(GetErrorMessage(response));
            Console.WriteLine("---------------------------");
            Console.WriteLine();

            Console.WriteLine("Press any key to Exit...");
            Console.ReadKey();
        }

        private static string GetErrorMessage(HttpResponseMessage response)
        {
            string text = response.Content.ReadAsStringAsync().Result;
            dynamic value = JObject.Parse(text);
            return value["error"] != null ? value.error.message : value.Message;
        }

        private static JObject GetFirstJsonItem(HttpResponseMessage response)
        {
            string text = response.Content.ReadAsStringAsync().Result;
            dynamic values = JToken.Parse(text);
            return values is JArray ? values[0] : values.value[0];
        }

        private static HttpResponseMessage SendODataRequest(string expandClause, string user, string roles)
        {
            HttpClient client = new HttpClient();
            string url = string.Format("{0}?$expand={1}", odataUrl, expandClause);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Identity", user);
            request.Headers.Add("X-Roles", roles);
            return client.SendAsync(request).Result;
        }

        private static HttpResponseMessage SendWebApiRequest(string expandClause, string user, string roles)
        {
            HttpClient client = new HttpClient();
            string url = string.Format("{0}?$expand={1}", webApiUrl, expandClause);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-Identity", user);
            request.Headers.Add("X-Roles", roles);
            return client.SendAsync(request).Result;
        }
    }
}
