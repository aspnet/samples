using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using CustomModelBinderSample.ModelBinders;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Converters;
using Owin;

namespace CustomModelBinderSample
{
    class Program
    {
        private static HttpClient client;

        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9095";

            using (WebApp.Start<Program>(url: baseAddress))
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(baseAddress);

                // Search for a book in the category 'Books' and having the word 'CLR' in it
                SendRequest("/api/products/search?categories=books&searchterm=CLR");

                // Search for products in categories 'Sports' and 'Clothing' and having the word 'Contoso' in it's name
                SendRequest("/api/products/search?categories=sports&categories=clothing&searchterm=contoso");

                // Search for all products having the word 'Contoso' in it's name. Notice that we are not
                // supplying any category information
                SendRequest("/api/products/search?searchterm=contoso");

                // NOTE:
                // Search for all products having the word 'Contoso' in it's name. Notice that categories is specified
                // as 'categories=' in query string and in this case the 'EmptyCollectionModelBinder' would create 
                // an empty collection. So the search is for the word 'Contoso' in all categories.
                // This also is a workaround for the following issue:
                // https://aspnetwebstack.codeplex.com/workitem/2067
                SendRequest("/api/products/search?categories=&searchterm=contoso");

                // Provide a non existing category and expect that model state fails
                SendRequest("/api/products/search?categories=NonExistingCategory&searchterm=contoso");
            }
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            // Get the default model binder provider which handles model binding to collection
            // and then wrap this into the custom implementation of collection model binder provider.
            // The idea here is that we are going to let the default model binder do the job of binding
            // for most of the scenarios and would only handle few cases where we want to change the
            // default behavior(https://aspnetwebstack.codeplex.com/workitem/2067)
            List<ModelBinderProvider> providers = config.Services.GetModelBinderProviders().ToList();

            // Since the order of model binder providers in the list is important (performance being one of the reasons),
            // we are updating the existing location of the default provider instead of adding it to the end of the list.
            int index = providers.FindIndex(prov => prov is CollectionModelBinderProvider);
            providers[index] = new EmptyCollectionModelBinderProvider((CollectionModelBinderProvider)providers[index]);

            // Since there are multiple services registered for the type "ModelBinderProvider",
            // we need to make sure to replace the range of these services.
            config.Services.ReplaceRange(typeof(ModelBinderProvider), providers);

            // By default Json.net serailizer writes an enum's value than name, so adding this converter
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            config.MapHttpAttributeRoutes();

            appBuilder.UseWebApi(config);
        }

        private static void SendRequest(string url)
        {
            HttpResponseMessage response = client.GetAsync(url).Result;

            Console.WriteLine("------------------------------------");
            Console.WriteLine("Request Uri: {0}", response.RequestMessage.RequestUri);
            Console.WriteLine("Response:");
            Console.WriteLine(response.ToString());

            if (response.Content != null)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}


