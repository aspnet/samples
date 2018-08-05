using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FileUploadStress.Controllers
{
    class StreamProvider : MultipartFormDataStreamProvider
    {
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            // Check that we have a content type
            if (headers != null && headers.ContentType != null)
            {
                MediaTypeHeaderValue contentType = headers.ContentType;

            }
            else
            {
                // Default to base behavior
                return base.GetLocalFileName(headers);
            }
        }
    }
}
