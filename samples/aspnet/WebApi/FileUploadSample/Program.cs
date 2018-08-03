using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace FileUploadSample
{
    /// <summary>
    /// This sample illustrates how to upload files to an ApiController using HttpClient
    /// </summary>
    class Program
    {
        const int BufferSize = 1024;

        static readonly string _baseAddress = "http://localhost:50231/";
        static readonly string _filename = "Sample.xml";

        static void Main(string[] args)
        {
            IDisposable server = null;

            try
            {
                server = WebApp.Start<Program>(url: _baseAddress);

                // Set up server configuration
                Console.WriteLine("Listening on " + _baseAddress);

                // Run HttpClient issuing requests
                RunClient();

                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
            finally
            {
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

            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

            appBuilder.UseWebApi(config);
        }

        /// <summary>
        /// Runs an HttpClient uploading files using MIME multipart to the controller.
        /// The client uses a progress notification message handler to report progress. 
        /// </summary>
        static async void RunClient()
        {
            // Create a progress notification handler
            ProgressMessageHandler progress = new ProgressMessageHandler();
            progress.HttpSendProgress += ProgressEventHandler;

            // Create an HttpClient and wire up the progress handler
            HttpClient client = HttpClientFactory.Create(progress);

            // Set the request timeout as large uploads can take longer than the default 2 minute timeout
            client.Timeout = TimeSpan.FromMinutes(20);

            // Open the file we want to upload and submit it
            using (FileStream fileStream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true))
            {
                // Create a stream content for the file
                StreamContent content = new StreamContent(fileStream, BufferSize);

                // Create Multipart form data content, add our submitter data and our stream content
                MultipartFormDataContent formData = new MultipartFormDataContent();
                formData.Add(new StringContent("Me"), "submitter");
                formData.Add(content, "filename", _filename);

                // Post the MIME multipart form data upload with the file
                Uri address = new Uri(_baseAddress + "api/fileupload");
                HttpResponseMessage response = await client.PostAsync(address, formData);

                FileResult result = await response.Content.ReadAsAsync<FileResult>();
                Console.WriteLine("{0}Result:{0}  Filename:  {1}{0}  Submitter: {2}", Environment.NewLine, result.FileNames.FirstOrDefault(), result.Submitter);
            }
        }

        static void ProgressEventHandler(object sender, HttpProgressEventArgs eventArgs)
        {
            // The sender is the originating HTTP request   
            HttpRequestMessage request = sender as HttpRequestMessage;

            // Write different message depending on whether we have a total length or not   
            string message;
            if (eventArgs.TotalBytes != null)
            {
                message = String.Format("  Request {0} uploaded {1} of {2} bytes ({3}%)",
                    request.RequestUri, eventArgs.BytesTransferred, eventArgs.TotalBytes, eventArgs.ProgressPercentage);
            }
            else
            {
                message = String.Format("  Request {0} uploaded {1} bytes",
                    request.RequestUri, eventArgs.BytesTransferred, eventArgs.TotalBytes, eventArgs.ProgressPercentage);
            }

            // Write progress message to console   
            Console.WriteLine(message);
        }
    }
}
