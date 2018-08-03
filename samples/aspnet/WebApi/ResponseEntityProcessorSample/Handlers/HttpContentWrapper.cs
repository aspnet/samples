using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResponseEntityProcessorSample.Handlers
{
    /// <summary>
    /// Help class for wrapping an inner <see cref="HttpContent"/> for the purpose of processing the data.
    /// </summary>
    public abstract class HttpContentWrapper : HttpContent
    {
        protected HttpContentWrapper(HttpContent innerContent)
        {
            if (innerContent == null)
            {
                throw new ArgumentNullException("innertContent");
            }

            InnerContent = innerContent;

            // Copy headers from inner to outer content
            foreach (KeyValuePair<string, IEnumerable<string>> header in InnerContent.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        protected HttpContent InnerContent { get; private set; }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return InnerContent.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            long? contentLength = InnerContent.Headers.ContentLength;
            if (contentLength.HasValue)
            {
                length = contentLength.Value;
                return true;
            }

            length = -1;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                InnerContent.Dispose();
            }
        }
    }
}
