using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ResponseEntityProcessorSample.Handlers
{
    /// <summary>
    /// This <see cref="DelegatingHandler"/> checks that there is a response entity
    /// and if so plugs in a <see cref="HttpContentProcessor"/> instance which
    /// copies the response entity to a local file and then enables post-processing
    /// on that file. 
    /// </summary>
    public class ResponseEntityHandler : DelegatingHandler
    {
        private string _outputPath;

        public ResponseEntityHandler(string outputPath)
        {
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }
            _outputPath = outputPath;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Wait for the response
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // Check if there is any content then plug in our content processor wrapper
            if (response.Content != null)
            {
                // By plugging in a HttpContentProcessor we can pass a Func<string, Task>
                // which gets the file name of the local file and then processes it asynchronously.
                response.Content = new HttpContentProcessor(response.Content, _outputPath,
                    (fileName) =>
                    {
                        // For this sample we only check the local file and then return.
                        FileInfo fileInfo = new FileInfo(fileName);
                        Console.WriteLine("  HttpMessageHandler saved response in local file: Size: {0,8} Name: {1}.", fileInfo.Length, fileInfo.FullName);
                        Console.WriteLine("  File will automatically get deleted upon completion of processing.");
                        Console.WriteLine();
                        return Task.FromResult(true);
                    });
            }

            return response;
        }
    }
}
