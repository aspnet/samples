using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Extensions;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using Owin;

namespace ODataCamelCaseSample
{
    public class Program
    {
        private static readonly string _baseAddress = "http://localhost:12345";
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _namespace = typeof(Employee).Namespace;

        public static void Main(string[] args)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            using (WebApp.Start(_baseAddress, Configuration))
            {
                Console.WriteLine("Listening on " + _baseAddress);

                // Query the metadata
                QueryMetadata();

                // Query an entity set
                QueryEmployees();

                // Query an property
                QueryName();

                // Add a new employee
                AddEmployee();

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        public static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.MapODataServiceRoute(routeName: "OData", routePrefix: "odata", model: ODataModels.GetModel());
            builder.UseWebApi(config);
        }

        public static void QueryMetadata()
        {
            string requestUri = _baseAddress + "/odata/$metadata";
            using (HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result)
            {
                response.EnsureSuccessStatusCode();
                string metadata = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("\nThe metadata is: \n {0}", metadata);
            }
        }

        public static void QueryEmployees()
        {
            string requestUri = _baseAddress + "/odata/Employees?$select=id,name,address&$filter=name eq 'Name1'";
            using (HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result)
            {
                response.EnsureSuccessStatusCode();
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine("\nEmployees whose name is 'Name1' are:");
                Console.WriteLine(result);
            }
        }

        public static void QueryName()
        {
            string requestUri = _baseAddress + "/odata/Employees(1)/name";

            using (HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result)
            {
                response.EnsureSuccessStatusCode();
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine("\nThe name of Employees(1) is:");
                Console.WriteLine(result);
            }
        }

        public static void AddEmployee()
        {
            string requestUri = _baseAddress + "/odata/Employees";

            JObject postContent = JObject.Parse(@"{'id':1,
                    'name':'Ben',
                    'gender':'Male',
                    'address':{'city':'Shanghai','street':'Zixing'},
                    }");
            using (HttpResponseMessage response = _httpClient.PostAsJsonAsync(requestUri, postContent).Result)
            {
                string content = response.Content.ReadAsStringAsync().Result;

                response.EnsureSuccessStatusCode();
                JObject result = JObject.Parse(content);
                Console.WriteLine("\nThe newly added employee is:");
                Console.WriteLine(result);
            }
        }
    }
}
