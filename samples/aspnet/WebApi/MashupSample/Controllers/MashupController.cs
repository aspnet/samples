using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using MashupSample.Models;
using Newtonsoft.Json.Linq;

namespace MashupSample.Controllers
{
    /// <summary>
    /// This <see cref="ApiController"/> aggregates information from http://www.digg.com and 
    /// http://delicious.com and presents the aggregation in response to a GET request.
    /// The information is retrieved asynchronously from the two sites so that no threads are
    /// blocked.
    /// </summary>
    public class MashupController : ApiController
    {
        // Reuse the same HttpClient instance 
        private static readonly HttpClient _client = new HttpClient();

        public async Task<List<Story>> GetContent(string topic)
        {
            List<Story> result = new List<Story>();

            // Check that we have a topic or return empty list
            if (topic == null)
            {
                return result;
            }

            // Isolate topic query to ensure we have a single term
            string queryToken = topic.Split(new char[] { '&' }).FirstOrDefault();

            // Submit async query requests and process responses in parallel
            List<Story>[] queryResults = await Task.WhenAll(
                ExecuteDiggQuery(queryToken),
                ExecuteDeliciousQuery(queryToken));

            // Aggregate results from digg and delicious
            foreach (List<Story> queryResult in queryResults)
            {
                result.AddRange(queryResult);
            }

            return result;
        }

        private static async Task<List<Story>> ExecuteDiggQuery(string queryToken)
        {
            List<Story> result = new List<Story>();

            // URI query for a basic digg query -- see http://developers.digg.com/documentation
            string query = string.Format("http://services.digg.com/2.0/search.search?query={0}", queryToken);

            // Submit async request 
            HttpResponseMessage diggResponse = await _client.GetAsync(query);

            // Read result using JToken and create response
            if (diggResponse.IsSuccessStatusCode && IsJson(diggResponse.Content))
            {
                JToken diggResult = await diggResponse.Content.ReadAsAsync<JToken>();
                foreach (var story in diggResult["stories"] as JArray)
                {
                    result.Add(new Story
                    {
                        Source = "digg",
                        Description = story["title"].Value<string>()
                    });
                }
            }

            return result;
        }

        private static async Task<List<Story>> ExecuteDeliciousQuery(string queryToken)
        {
            List<Story> result = new List<Story>();

            // URI query for a basic delicious query -- see http://delicious.com/developers
            string query = string.Format("http://feeds.delicious.com/v2/json/tag/{0}", queryToken);

            // Submit async request 
            HttpResponseMessage deliciousResponse = await _client.GetAsync(query);

            // Read result using JToken and create response
            if (deliciousResponse.IsSuccessStatusCode && IsJson(deliciousResponse.Content))
            {
                JArray deliciousResult = await deliciousResponse.Content.ReadAsAsync<JArray>();
                foreach (var story in deliciousResult)
                {
                    result.Add(new Story
                    {
                        Source = "delicious",
                        Description = story["d"].Value<string>()
                    });
                }
            }

            return result;
        }

        private static bool IsJson(HttpContent content)
        {
            if (content == null || content.Headers == null || content.Headers.ContentType == null)
            {
                return false;
            }

            MediaTypeHeaderValue contentType = content.Headers.ContentType;
            return
                contentType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase) ||
                contentType.MediaType.Equals("text/json", StringComparison.OrdinalIgnoreCase);
        }
    }
}
