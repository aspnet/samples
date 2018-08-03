using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RelaySample.Controllers
{
    /// <summary>
    /// This sample controller functions as a "backend" service which the RelayController relays messages for.
    /// </summary>
    public class ContentController : ApiController
    {
        const int BufferSize = 32 * 1024;
        static readonly string _path = Path.Combine(Path.GetTempPath(), "WebApiSample");
        static readonly string _store = Path.Combine(_path, "StoreContent");

        static ContentController()
        {
            Directory.CreateDirectory(_path);
        }

        public Task<HttpResponseMessage> GetContent()
        {
            if (!File.Exists(_store))
            {
                return Task.FromResult(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            // Open file and read response from it. If read fails then return 503 Service Not Available           
            try
            {
                Console.WriteLine("Retrieving file from {0}", _store);

                // Create StreamContent from FileStream. FileStream will get closed when StreamContent is closed
                FileStream fStream = new FileStream(_store, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true);
                HttpResponseMessage response = Request.CreateResponse();
                response.Content = new StreamContent(fStream);
                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                return Task.FromResult(Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, e));
            }
        }

        public async Task<HttpResponseMessage> PutContent()
        {
            // Determine whether this is first time accessing the file so that we can return 201 Created.
            bool first = !File.Exists(_store);

            // Open file and write request to it. If write fails then return 503 Service Not Available
            try
            {
                Console.WriteLine("Saving file as {0}", _store);

                using (FileStream fStream = new FileStream(_store, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, useAsync: true))
                {
                    // Copy content asynchronously to local store
                    await Request.Content.CopyToAsync(fStream);
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, e);
            }

            // Return either 200 Ok or 201 Created response
            return Request.CreateResponse(first ? HttpStatusCode.Created : HttpStatusCode.OK);
        }
    }
}