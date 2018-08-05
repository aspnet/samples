using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResponseEntityProcessorSample.Handlers
{
    /// <summary>
    /// Wraps an inner <see cref="HttpContent"/> and forces the content to be written
    /// both to a local file as well as to the output stream when SerializeToStreamAsync
    /// is called.
    /// </summary>
    internal class HttpContentProcessor : HttpContentWrapper
    {
        private const int BufferSize = 8 * 1024;

        private string _outputPath;
        private Func<string, Task> _contentProcessor;

        public HttpContentProcessor(HttpContent innerContent, string outputPath, Func<string, Task> contentProcessor)
            : base(innerContent)
        {
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }
            if (contentProcessor == null)
            {
                throw new ArgumentNullException("contentProcessor");
            }
            _outputPath = outputPath;
            _contentProcessor = contentProcessor;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            // First write out content to local file asynchronously
            string fileName = Guid.NewGuid().ToString("D");
            string filePath = Path.Combine(_outputPath, fileName);
            using (FileStream fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, useAsync: true))
            {
                await InnerContent.CopyToAsync(fStream);
            }

            // Now write out content to actual output stream
            await InnerContent.CopyToAsync(stream);

            // Invoke content processor so that it can process the content asynchonously
            try
            {
                await _contentProcessor(filePath);
            }
            finally
            {
                // We are done with the local file and can delete it.
                File.Delete(filePath);
            }
        }
    }
}
