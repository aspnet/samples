using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Principal;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using CustomParameterBindingSample.Models;
using Microsoft.Owin.Hosting;
using Owin;

namespace CustomParameterBindingSample
{
    /// <summary>
    /// This sample shows how to customize the parameter binding process by registering 
    /// custom ParameterBinding for certain types. 
    /// </summary>
    class Program
    {
        static readonly string _baseAddress = "http://localhost:50231/";

        static void Main(string[] args)
        {
            IDisposable server = null;

            try
            {
                server = WebApp.Start<Program>(url: _baseAddress);

                Console.WriteLine("Listening on " + _baseAddress + "...\n");

                // RunClient
                RunClient();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
            }
            finally
            {
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();

                if (server != null)
                {
                    // Stop listening
                    server.Dispose();
                }
            }
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute("Default", "{controller}/{action}", new { controller = "Home" });

            // Register an action to create custom ParameterBinding
            config.ParameterBindingRules.Insert(0, GetCustomParameterBinding);

            appBuilder.UseWebApi(config);
        }

        private static void RunClient()
        {
            // start the client
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            HttpResponseMessage response;

            // How to bind a parameter not coming from request
            response = client.GetAsync("/Home/BindPrincipal").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to binding a complex type either from body 
            ObjectContent oc = new ObjectContent(typeof(TestItem), new TestItem() { Name = "HongmeiFromMessageBody" }, new JsonMediaTypeFormatter());
            response = client.PostAsync("/Home/BindCustomComplexTypeFromUriOrBody", oc).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to bind a complex type either from uri for the same action 
            response = client.GetAsync("/Home/BindCustomComplexTypeFromUriOrBody?Name=HongmeiFromUri").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to bind a complex type with renamed property from Name to $Name
            response = client.GetAsync("/Home/BindCustomComplexTypeFromUriWithRenamedProperty?$Name=HongmeiFromUriWithRenamedProperty").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to bind multiple parameters from request for POST
            StringContent content = new StringContent("firstname=Hongmei&lastname=Ge", Encoding.UTF8, "application/x-www-form-urlencoded");
            response = client.PostAsync("/Home/PostMultipleParametersFromBody", content).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();
        }

        public static HttpParameterBinding GetCustomParameterBinding(HttpParameterDescriptor descriptor)
        {
            if (descriptor.ParameterType == typeof(IPrincipal))
            {
                return new PrincipalParameterBinding(descriptor);
            }
            else if ( descriptor.ParameterType == typeof(TestItem))
            {
                return new FromUriOrBodyParameterBinding(descriptor);
            }
            else if (descriptor.ParameterType == typeof(TestItemRenameProperty))
            {
                return new CustomModelBinderWithPropertyName(descriptor);
            }
            else if ( descriptor.ParameterType == typeof(string) )
            {
                return new MultipleParameterFromBodyParameterBinding(descriptor);
            }
            
            // any other types, let the default parameter binding handle
            return null;
        }
    }
}
