using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    /// This sample client tries out the FilesController exposed by the AzureBlobsFileUpload project in this solution.
    /// It first issues a MIME multipart POST request and then a GET request for the location in the store.
    /// </summary>
    class Program
    {
        static readonly Uri _addres = new Uri("http://localhost:19446/api/files");

        static async Task RunClientAsync()
        {
            HttpClient client = new HttpClient();

            // Issue MIME multipart POST request with a MIME multipart message containing a single
            // body part with StringContent.
            StringContent content = new StringContent("Hello World", Encoding.UTF8, "text/plain");
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(content, "file", "HelloWorld.txt");

            Console.WriteLine("Uploading data to store...");
            HttpResponseMessage postResponse = await client.PostAsync(_addres, formData);

            postResponse.EnsureSuccessStatusCode();
            JArray postResult = await postResponse.Content.ReadAsAsync<JArray>();
            string location = postResult[0].Value<string>("Location");

            // Issue GET request to get the content back from the store
            Console.WriteLine("Retrieving data from store: {0}", location);
            HttpResponseMessage getResponse = await client.GetAsync(location);

            getResponse.EnsureSuccessStatusCode();
            string result = await getResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Received response: {0}", result);
        }

        static void Main(string[] args)
        {
            RunClientAsync().Wait();

            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
    }
}
