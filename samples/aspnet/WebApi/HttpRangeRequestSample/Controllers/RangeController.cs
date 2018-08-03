using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace HttpRangeRequestSample.Controllers
{
    /// <summary>
    /// This controller illustrates support for HTTP range requests with ASP.NET Web API using the <see cref="ByteRangeStreamContent"/>.
    /// </summary>
    public class RangeController : ApiController
    {
        // Sample content used to demonstrate range requests
        private static readonly byte[] _content = Encoding.UTF8.GetBytes("abcdefghijklmnopqrstuvwxyz");

        // Content type for our body
        private static readonly MediaTypeHeaderValue _mediaType = MediaTypeHeaderValue.Parse("text/plain");

        public HttpResponseMessage Get()
        {
            // A MemoryStream is seekable allowing us to do ranges over it. Same goes for FileStream.
            MemoryStream memStream = new MemoryStream(_content);

            // Check to see if this is a range request (i.e. contains a Range header field)
            // Range requests can also be made conditional using the If-Range header field. This can be 
            // used to generate a request which says: send me the range if the content hasn't changed; 
            // otherwise send me the whole thing.
            if (Request.Headers.Range != null)
            {
                try
                {
                    HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                    partialResponse.Content = new ByteRangeStreamContent(memStream, Request.Headers.Range, _mediaType);
                    return partialResponse;
                }
                catch (InvalidByteRangeException invalidByteRangeException)
                {
                    return Request.CreateErrorResponse(invalidByteRangeException);
                }
            }
            else
            {
                // If it is not a range request we just send the whole thing as normal
                HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
                fullResponse.Content = new StreamContent(memStream);
                fullResponse.Content.Headers.ContentType = _mediaType;
                return fullResponse;
            }
        }
    }
}
