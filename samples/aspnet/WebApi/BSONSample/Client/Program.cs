
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private static Uri _baseAddress = new Uri("http://localhost:38734/");

        static void Main(string[] args)
        {
            try
            {
                RunAsync().Wait();
            }
            finally
            {
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();
            }
        }

        static async Task RunAsync()
        {
            MediaTypeFormatter formatter = new BsonMediaTypeFormatter();
            MediaTypeFormatter[] formatters = new MediaTypeFormatter[] { formatter, };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;

                // All responses in BSON format
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/bson"));

                // POST api/MyData to create api/MyData/1 (send and receive MyData)
                MyData data = new MyData
                {
                    Name = "First",
                    Decimal = -90998887666m,
                    Double = 37.777777777777777777d,
                    Long = 238470192387401239L,
                    TimeSpan = TimeSpan.Zero,
                };
                Uri[] uris = new Uri[2];

                Console.WriteLine("POSTing to '/api/MyData'");
                HttpResponseMessage response = await client.PostAsync<MyData>("api/MyData", data, formatter);
                response.EnsureSuccessStatusCode();
                uris[0] = response.Headers.Location;
                Console.WriteLine("  ... success; created resource at '{0}'", uris[0].PathAndQuery);
                Console.WriteLine("  ... response was {0} bytes", response.Content.Headers.ContentLength);

                // Unused but could confirm returned data is unchanged (MyDataController makes no changes on POST)
                data = await response.Content.ReadAsAsync<MyData>(formatters);

                // POST api/MyData to create api/MyData/2
                data = new MyData
                {
                    Name = "Second",
                    Decimal = 999999999999m,
                    Double = 3.3498712034e37d,
                    Long = -91348701321872304L,
                    TimeSpan = new TimeSpan(0, 12, 13, 14, 123),
                };

                Console.WriteLine("POSTing to '/api/MyData'");
                response = await client.PostAsync<MyData>("api/MyData", data, formatter);
                response.EnsureSuccessStatusCode();
                uris[1] = response.Headers.Location;
                Console.WriteLine("  ... success; created resource at '{0}'", uris[1].PathAndQuery);

                // GET api/MyData (receive enumeration of MyData)
                Console.WriteLine("GETting from '/api/MyData'");
                response = await client.GetAsync("api/MyData");
                response.EnsureSuccessStatusCode();
                Console.WriteLine("  ... success; response was {0} bytes", response.Content.Headers.ContentLength);

                MyData[] allData = await response.Content.ReadAsAsync<MyData[]>(formatters);
                foreach (MyData returned in allData)
                {
                    PrettyPrint("  ...", returned);
                }

                // DELETE api/MyData/x (receive MyData)
                foreach (Uri uri in uris)
                {
                    Console.WriteLine("DELETing resource at '{0}'", uri.PathAndQuery);
                    response = await client.DeleteAsync(uri);
                    response.EnsureSuccessStatusCode();

                    data = await response.Content.ReadAsAsync<MyData>(formatters);
                    PrettyPrint("  ... success;", data);
                }
            }
        }

        private static void PrettyPrint(string action, MyData data)
        {
            Console.WriteLine(
                "{0} returned MyData {{" + Environment.NewLine +
                "    Id={1}," + Environment.NewLine +
                "    Name=\"{2}\"," + Environment.NewLine +
                "    Decimal={3}," + Environment.NewLine +
                "    Double={4}," + Environment.NewLine +
                "    Long={5}," + Environment.NewLine +
                "    TimeSpan={6} }}",
                action, data.Id, data.Name, data.Decimal, data.Double, data.Long, data.TimeSpan);
        }
    }
}
