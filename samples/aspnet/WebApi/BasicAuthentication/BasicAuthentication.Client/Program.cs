using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BasicAuthentication.Models;

namespace BasicAuthentication.Client
{
    class Program
    {
        const string ServerAddress = "http://localhost:1095/";

        static void Main()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            Console.WriteLine("Press ENTER to start . . .");
            WaitForEnter();
            Console.WriteLine();

            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Sending request with no credentials:");
                await TryRequestAsync(client, authorization: null);
                Console.WriteLine("========================================");

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("========================================");
                Console.WriteLine("Sending request with credentials:");
                await TryRequestAsync(client, CreateBasicCredentials("SampleUser", "secret"));
                Console.WriteLine("========================================");
                Console.WriteLine();
            }

            Console.WriteLine("Press ENTER to exit . . .");
            WaitForEnter();
        }

        static async Task TryRequestAsync(HttpClient client, AuthenticationHeaderValue authorization)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ServerAddress))
            {
                request.Headers.Authorization = authorization;
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    Console.WriteLine("{0} {1}", (int)response.StatusCode, response.ReasonPhrase);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return;
                    }

                    Console.WriteLine();

                    HomeModel model = await response.Content.ReadAsAsync<HomeModel>();
                    Console.WriteLine("UserName: {0}", model.UserName);
                    Console.WriteLine("Claims:");

                    foreach (ClaimModel claim in model.Claims)
                    {
                        Console.WriteLine("{0} => {1}", claim.Type, claim.Value);
                    }
                }
            }
        }

        static AuthenticationHeaderValue CreateBasicCredentials(string userName, string password)
        {
            string toEncode = userName + ":" + password;
            // The current HTTP specification says characters here are ISO-8859-1.
            // However, the draft specification for the next version of HTTP indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            byte[] toBase64 = encoding.GetBytes(toEncode);
            string parameter = Convert.ToBase64String(toBase64);

            return new AuthenticationHeaderValue("Basic", parameter);
        }

        static void WaitForEnter()
        {
            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) ;
        }
    }
}
