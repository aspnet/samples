using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using Owin;

namespace ODataComplexTypeInheritanceSample
{
    public class Program
    {
        private static readonly string _baseAddress = String.Format("http://{0}:12345", Environment.MachineName);
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _namespace = typeof(Window).Namespace;

        public static void Main(string[] args)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            using (WebApp.Start(_baseAddress, Configuration))
            {
                Console.WriteLine("Listening on " + _baseAddress);
                string requestUri = "";
                HttpResponseMessage response = null;

                // The complex type Shape is an abstract type, in the EDM model, its IsAbstract is true.
                // The complex type Circle and Polygon derive from Shape and
                // the complex type Rectangle derives from Polygon.
                requestUri = _baseAddress + "/odata/$metadata";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response, true);

                // The property DefaultShape in Windows is declared as Shape, and in the instance Windows(1)
                // it is actually a Ploygon.
                // The property OptionalShapes is a collection of Shape, in the instance the 3 types of Shape 
                // are included.
                requestUri = _baseAddress + "/odata/Windows(1)";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response);

                // Get a property defined in the derived Complex type.
                requestUri = _baseAddress + "/odata/Windows(1)/CurrentShape/ODataComplexTypeInheritanceSample.Circle/Radius";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response);

                // Function that returns a base complex type
                requestUri = _baseAddress + "/odata/Windows(1)/ODataComplexTypeInheritanceSample.GetTheLastOptionalShape()";
                Comment("GET " + requestUri);
                response = Get(requestUri);
                Comment(response);

                // Action that takes in a base complex type
                requestUri = _baseAddress + "/odata/Windows(1)/ODataComplexTypeInheritanceSample.AddOptionalShape";
                Comment("POST " + requestUri);
                response = ActionCall(requestUri);
                Comment(response);

                // This method illustrate how to add an entity which contains derived complex type.
                requestUri = _baseAddress + "/odata/Windows";
                Comment("POST " + requestUri);
                response = Post(requestUri);
                Comment(response);

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        public static void Configuration(IAppBuilder builder)
        {
            HttpConfiguration config = new HttpConfiguration();
            IEdmModel edmModel = ODataModels.GetModel();
            config.MapODataServiceRoute(routeName: "OData", routePrefix: "odata", model: edmModel);
            builder.UseWebApi(config);
        }

        private static HttpResponseMessage Get(string requestUri)
        {
            HttpResponseMessage response = _httpClient.GetAsync(requestUri).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        private static HttpResponseMessage Post(string requestUri)
        {
            string content = @"{
    'Id':0,
    'Name':'Name4',
    'CurrentShape':
    {
        '@odata.type':'#ODataComplexTypeInheritanceSample.Circle',  
        'Radius':10,
        'Center':{'X':1,'Y':2},
        'HasBorder':true
    },
    'OptionalShapes':
    [
        {
            '@odata.type':'#ODataComplexTypeInheritanceSample.Polygon',
            'HasBorder':true,
            'Vertexes':
            [
                {
                  'X':0,'Y':0
                },
                {
                  'X':2,'Y':0
                },
                {
                  'X':2,'Y':2
                }
           ]
        }
    ]
}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        private static HttpResponseMessage ActionCall(string requestUri)
        {
            string content = @"
{
    'shape':
    {
        '@odata.type':'#ODataComplexTypeInheritanceSample.Polygon',
        'HasBorder':true,
        'Vertexes':[
            {
              'X':0,'Y':0
            },
            {
              'X':2,'Y':0
            },
            {
              'X':2,'Y':2
            },
            {
              'X':0,'Y':2
            }
        ]
    }
}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }

        private static void Comment(string message)
        {
            Console.WriteLine(message);
        }

        private static void Comment(HttpResponseMessage response, bool metadataResponse = false)
        {
            Console.WriteLine(response);
            if (metadataResponse)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
            }
            else
            {
                JObject result = response.Content.ReadAsAsync<JObject>().Result;
                Console.WriteLine(result);
            }
        }
    }
}
