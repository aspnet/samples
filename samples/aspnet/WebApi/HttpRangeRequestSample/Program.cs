using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace HttpRangeRequestSample
{
    /// <summary>
    /// This sample illustrates supporting HTTP range requests with ASP.NET Web API using the <see cref="ByteRangeStreamContent"/>.
    /// Please see ReadMe.txt for details
    /// </summary>
    class Program
    {
        static readonly string baseAddress = "http://localhost:50231";

        static void Main(string[] args)
        {
            using (WebApp.Start<Program>(url: baseAddress))
            {
                Console.WriteLine("Listening at " + baseAddress);

                RunClient();

                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
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
        /// Runs an HttpClient issuing sample requests against controller using Range requests.
        /// </summary>
        static void RunClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress + "/api/range");

            // Get the full content without any ranges
            using (HttpResponseMessage response = client.GetAsync("").Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Full Content without ranges: '{0}'\n", content);
            }

            // Get the first byte 
            HttpRequestMessage firstByteRequest = new HttpRequestMessage();
            firstByteRequest.Headers.Range = new RangeHeaderValue(0, 0);
            using (HttpResponseMessage response = client.SendAsync(firstByteRequest).Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Range '{0}' requesting the first byte: '{1}'\n", firstByteRequest.Headers.Range, content);
            }

            // Get the last byte 
            HttpRequestMessage lastByteRequest = new HttpRequestMessage();
            lastByteRequest.Headers.Range = new RangeHeaderValue(null, 1);
            using (HttpResponseMessage response = client.SendAsync(lastByteRequest).Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Range '{0}' requesting the last byte: '{1}'\n", lastByteRequest.Headers.Range, content);
            }

            // Get from byte 4 and up 
            HttpRequestMessage fourAndUpByteRequest = new HttpRequestMessage();
            fourAndUpByteRequest.Headers.Range = new RangeHeaderValue(4, null);
            using (HttpResponseMessage response = client.SendAsync(fourAndUpByteRequest).Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Range '{0}' requesting byte 4 and up: '{1}'\n", fourAndUpByteRequest.Headers.Range, content);
            }

            // Get the first and the last byte
            HttpRequestMessage firstAndLastByteRequest = new HttpRequestMessage();
            RangeHeaderValue firstAndLastByteRange = new RangeHeaderValue();
            firstAndLastByteRange.Ranges.Add(new RangeItemHeaderValue(0, 0));
            firstAndLastByteRange.Ranges.Add(new RangeItemHeaderValue(null, 1));
            firstAndLastByteRequest.Headers.Range = firstAndLastByteRange;
            using (HttpResponseMessage response = client.SendAsync(firstAndLastByteRequest).Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Range '{0}' requesting first and last byte:\n{1}\n", firstAndLastByteRange, content);
            }

            // Get the first, mid four, and the last byte
            HttpRequestMessage firstMidAndLastByteRequest = new HttpRequestMessage();
            RangeHeaderValue firstMidAndLastByteRange = new RangeHeaderValue();
            firstMidAndLastByteRange.Ranges.Add(new RangeItemHeaderValue(0, 0));
            firstMidAndLastByteRange.Ranges.Add(new RangeItemHeaderValue(12, 15));
            firstMidAndLastByteRange.Ranges.Add(new RangeItemHeaderValue(null, 1));
            firstMidAndLastByteRequest.Headers.Range = firstMidAndLastByteRange;
            using (HttpResponseMessage response = client.SendAsync(firstMidAndLastByteRequest).Result)
            {
                response.EnsureSuccessStatusCode();
                string content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Range '{0}' requesting first, mid four, and last byte:\n{1}\n", firstMidAndLastByteRange, content);
            }

            // Ask for a non-matching range (byte 100 and up)
            HttpRequestMessage nonMatchingByteRequest = new HttpRequestMessage();
            nonMatchingByteRequest.Headers.Range = new RangeHeaderValue(100, null);
            using (HttpResponseMessage response = client.SendAsync(nonMatchingByteRequest).Result)
            {
                Console.WriteLine("Range '{0}' resulted in status code '{1}' with Content-Range header '{2}'",
                    nonMatchingByteRequest.Headers.Range, response.StatusCode, response.Content.Headers.ContentRange);
            }
        }
    }
}
