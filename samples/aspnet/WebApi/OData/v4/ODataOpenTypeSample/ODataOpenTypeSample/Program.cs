using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using Owin;

namespace ODataOpenTypeSample
{
    public class Program
    {
        private static readonly string _baseAddress = string.Format("http://{0}:12345", Environment.MachineName);
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _namespace = typeof(Account).Namespace;

        public static void Main(string[] args)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            using (WebApp.Start(_baseAddress, Configuration))
            {
                Console.WriteLine("Listening on " + _baseAddress);

                // The attribute OpenType is returned for the entity type Account and the complex type Address:
                // <EntityType Name="Account" OpenType="true">
                // <ComplexType Name="Address" OpenType="true">
                Comment("GET ~/$metadata");
                HttpResponseMessage response = QueryMetadata();
                string metadata = response.Content.ReadAsStringAsync().Result;
                Comment(response.ToString());
                Comment(metadata);

                // Dynamic property values are also returned, such as: 
                // Address.Country, Account.Emails, Account.Gender.
                Comment("GET ~/Accounts(1)");
                response = QueryAccount();
                Comment(response);

                // Dynamic property values are also returned, such as: 
                // Address.Country.
                Comment("GET ~/Accounts(1)/Address");
                response = QueryAddressFromAccount();
                Comment(response);

                // Demonstrate how dynamic properties are post to server. 
                Comment("POST ~/Accounts");
                response = AddAccount();
                Comment(response);

                // The original account will be replaced.
                Comment("PUT ~/Accounts(1)");
                response = PutAccount();
                Comment(response);

                // If a dynamic property is set to null, it will be deleted. 
                // And if a new dynamic property is on the wire, it will be added to the entity.
                Comment("PATCH ~/Accounts(1)");
                response = PatchAccount();
                Comment(response);

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        public static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapODataServiceRoute(routeName: "OData", routePrefix: "odata", model: GetModel());
            builder.UseWebApi(config);
        }

        public static HttpResponseMessage QueryMetadata()
        {
            string requestUri = _baseAddress + "/odata/$metadata";
            HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result;

            response.EnsureSuccessStatusCode();
            return response;
        }

        public static HttpResponseMessage AddAccount()
        {
            string requestUri = _baseAddress + "/odata/Accounts";
            // Gender and Emails are dynamic properties of Entity type Account
            // Country is a dynamic property of Complex type Address
            JObject postContent = JObject.Parse(@"
                    {
                        'Id':1,
                        'Name':'Ben',
                        'Address':{'City':'Shanghai','Street':'Zixing','Country':'China'},
                        'Gender@odata.type':'#ODataOpenTypeSample.Gender',
                        'Gender':'Female',
                        'Emails@odata.type':'#Collection(Edm.String)',
                        'Emails':['a@a.com','b@b.com'],
                    }");

            HttpResponseMessage response = _httpClient.PostAsJsonAsync(requestUri, postContent).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static HttpResponseMessage PutAccount()
        {
            string requestUri = _baseAddress + "/odata/Accounts(1)";

            // Emails is a dynamic property of Entity type Account
            // Zip is a dynamic property of Complex type Address
            JObject postContent = JObject.Parse(@"
                    {
                        'Id':1,
                        'Name':'Jinfu',
                        'Address':{'City':'Beijing','Street':'Changan','Zip':'200-099'},
                        'Emails@odata.type':'#Collection(Edm.String)',
                        'Emails':['d@d.com','e@e.com'],
                    }");

            HttpResponseMessage response = _httpClient.PutAsJsonAsync(requestUri, postContent).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static HttpResponseMessage PatchAccount()
        {
            string requestUri = _baseAddress + "/odata/Accounts(1)";

            // Emails is a dynamic property of Entity type Account
            // IsDefault is a dynamic property of Complex type Address
            JObject content = JObject.Parse(@"{
                        'Address':{'Street':'Changan','IsDefault':true},
                        'Gender@odata.type':'#ODataOpenTypeSample.Gender',
                        'Gender':null,
                        'Emails@odata.type':'#Collection(Edm.String)',
                        'Emails':[],
                        'Whatever@odata.type':'#Collection(Edm.Int32)',
                        'Whatever':[1,2,3],
                    }");
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri);
            request.Content = new StringContent(content.ToString());
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static HttpResponseMessage QueryAccount()
        {
            string requestUri = _baseAddress + "/odata/Accounts(1)";

            HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static HttpResponseMessage QueryAddressFromAccount()
        {
            string requestUri = _baseAddress + "/odata/Accounts(1)/Address";

            HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        private static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Account>("Accounts");
            builder.EnumType<Gender>();

            builder.Namespace = typeof(Account).Namespace;

            var edmModel = builder.GetEdmModel();
            return edmModel;
        }

        private static void Comment(string message)
        {
            Console.WriteLine(message);
        }

        private static void Comment(HttpResponseMessage response)
        {
            Console.WriteLine(response);
            JObject result = response.Content.ReadAsAsync<JObject>().Result;
            Console.WriteLine(result);
        }
    }
}
