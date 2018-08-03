using System;
using System.Net.Http;
using System.Text;

namespace DeltaJsonDeserialization.Client
{
    internal static class Program
    {
        private static readonly string[] PatchData = new string[]
        {
            "{ \"Bool\":true }",
            "{ \"Byte\":97 }",
            "{ \"Bytes\":\"SGVsbG8=\", \"Enum\":5  }",
            "{ \"DateTime\":\"2013-12-07T21:55:45.8677285Z\" }",
            "{ \"NullableDateTime\":\"2013-12-07T21:55:45.8687306Z\" }",
            "{ \"DateTimeOffset\":\"2013-12-07T21:55:45.8677285+00:00\" }",
            "{ \"NullableDateTimeOffset\":\"2013-12-07T21:55:45.8687306+00:00\" }",
            "{ \"Double\":1, \"String\":\"Hello\" }",
            "{ \"Double\":1.0 }",
            "{ \"NullableDouble\":1.0, \"Int\":10  }",
            "{ \"NullableInt\":9 }",
            "{ \"Enum\": \"Saturday\" }",
        };

        public static void Main(string[] args)
        {
            using (var client = new HttpClient() { BaseAddress = new Uri("http://localhost:21607/") })
            {
                foreach (var patchData in PatchData)
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), "/api/Patch"))
                    {
                        Console.WriteLine("Sending: {0}", patchData);
                        Console.WriteLine();
                        request.Content = new StringContent(patchData, Encoding.UTF8, "application/json");

                        using (var response = client.SendAsync(request).Result)
                        {
                            response.EnsureSuccessStatusCode();

                            var responseText = response.Content.ReadAsStringAsync().Result;
                            Console.WriteLine(responseText);
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }
                }
            }

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
