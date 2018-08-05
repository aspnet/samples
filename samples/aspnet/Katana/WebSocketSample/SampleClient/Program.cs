using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunSample().Wait();
            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        public static async Task RunSample()
        {
            ClientWebSocket websocket = new ClientWebSocket();

            string url = "ws://localhost:5000/";
            Console.WriteLine("Connecting to: " + url);
            await websocket.ConnectAsync(new Uri(url), CancellationToken.None);

            string message = "Hello World";
            Console.WriteLine("Sending message: " + message);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await websocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);

            byte[] incomingData = new byte[1024];
            WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(incomingData), CancellationToken.None);

            if (result.CloseStatus.HasValue)
            {
                Console.WriteLine("Closed; Status: " + result.CloseStatus + ", " + result.CloseStatusDescription);
            }
            else
            {
                Console.WriteLine("Received message: " + Encoding.UTF8.GetString(incomingData, 0, result.Count));
            }
        }
    }
}
