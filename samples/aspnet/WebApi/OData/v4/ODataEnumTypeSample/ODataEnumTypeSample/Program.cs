using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Extensions;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using Owin;

namespace ODataEnumTypeSample
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

                // Query employees whose gender is Male
                QueryEmployeesByGender(Gender.Male);

                // Query employees who have the access level of 'Read'
                QueryEmployeesByAccessLevel(AccessLevel.Read);

                // Tell whether employee with ID 1 has the access level 'Execute'
                HasAccessLevel(1, AccessLevel.Execute);

                // Add access level 'Read' to the employee with ID 1
                AddAccessLevel(1, AccessLevel.Read);

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
                Console.WriteLine("\nThe metadata is \n {0}", metadata);
            }
        }

        public static void QueryEmployeesByGender(Gender gender)
        {
            string uriTemplate = _baseAddress + "/odata/Employees?$filter=Gender eq {0}'{1}'";
            string requestUri = string.Format(uriTemplate, typeof(Gender).FullName, gender.ToString());
            using (HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result)
            {
                response.EnsureSuccessStatusCode();
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine("\nEmployees whose gender is '{0}' are:", gender.ToString());
                Console.WriteLine(result);
            }
        }

        public static void QueryEmployeesByAccessLevel(AccessLevel accessLevel)
        {
            string uriTemplate = _baseAddress + "/odata/Employees?$filter=AccessLevel has {0}'{1}'";
            string uriHas = string.Format(uriTemplate, typeof(AccessLevel).FullName, accessLevel.ToString());

            using (HttpResponseMessage response = _httpClient.GetAsync(uriHas).Result)
            {
                response.EnsureSuccessStatusCode();
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine("\nEmployees who has the access level '{0}' are:", accessLevel.ToString());
                Console.WriteLine(result);
            }
        }

        public static void AddEmployee()
        {
            string requestUri = _baseAddress + "/odata/Employees";

            JObject postContent = JObject.Parse(@"{'ID':1,
                    'Name':'Ben',
                    'Gender':'Male',
                    'AccessLevel':'Read,Write',
                    }");
            using (HttpResponseMessage response = _httpClient.PostAsJsonAsync(requestUri, postContent).Result)
            {
                response.EnsureSuccessStatusCode();
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine("\nThe newly added employee is:");
                Console.WriteLine(result);
            }
        }

        public static void AddAccessLevel(int id, AccessLevel accessLevel)
        {
            var requestUri = _baseAddress + string.Format("/odata/Employees({0})/{1}.AddAccessLevel", id, _namespace);
            string body = string.Format(@"{{'AccessLevel':'{0}'}}", accessLevel.ToString());
            JObject postContent = JObject.Parse(body);

            using (HttpResponseMessage response = _httpClient.PostAsJsonAsync(requestUri, postContent).Result)
            {
                response.EnsureSuccessStatusCode();
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine("\nThe new access level of employee with ID '{0}' is:", id);
                Console.WriteLine(result);
            }
        }

        public static void HasAccessLevel(int id, AccessLevel accessLevel)
        {
            string uriTemplate = _baseAddress + "/odata/HasAccessLevel(ID={0},AccessLevel={1}'{2}')";
            string requestUri = string.Format(uriTemplate, id, typeof(AccessLevel).FullName, accessLevel.ToString());
            using (HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result)
            {
                response.EnsureSuccessStatusCode();
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine("\nEmployee with ID '{0}' has access level '{1}': ", id, accessLevel.ToString());
                Console.WriteLine(result);
            }
        }
    }
}
