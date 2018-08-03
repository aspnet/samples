HttpRangeRequestSample
----------------------

ByteRangeStreamContent provides support for generating byte range views over a seekable stream 
as defined by HTTP/1.1 range requests. The ByteRangeStreamContent can be used to generate 
HTTP/1.1 206 (Partial Content) responses where the client only requests one or more ranges of 
the response and not the complete response.

Given an inner stream and a range specification, the ByteRangeStreamContent creates a view of 
the inner stream reflecting the range specification. If there is only one range specified, e.g.

 Range: bytes 0-9

The resulting ByteRangeStreamContent will include a Content-Range header along with the other
HttpContent headers, e.g:

 Content-type: application/pdf
 Content-Range: bytes 0-9/100

indicating that the first 10 bytes of a total of 100 bytes are included. If there are more than 
one range specified, e.g.

 Range: 0-9,90-99

then the resulting ByteRangeStreamContent will contain a “multipart/byteranges” response carrying 
the set of ranges, e.g.

 Content-type: multipart/byteranges; boundary=THIS_STRING_SEPARATES

 --THIS_STRING_SEPARATES
 Content-type: application/pdf
 Content-range: bytes 0-9/100

 ...the first range...
 --THIS_STRING_SEPARATES
 Content-type: application/pdf
 Content-range: bytes 90-99/100

 ...the second range
 --THIS_STRING_SEPARATES--

If the ranges requested are not satisfiable then ByteRangeStreamContent throws a special exception for generating
a 416 (Requested Range Not Satisfiable) with a Content-Range header indicating the current extent of the resource.

  HTTP/1.1 416 Requested Range Not Satisfiable
  Content-Range: bytes */26

The ByteRangeStreamContent can also be used as part of scenarios serving conditional range requests containing an 
If-Range header field meaning “send me the following range but only if the ETag matches; otherwise send me the 
whole response.”

For a detailed description of HTTP/1.1 ranges, please see "Hypertext Transfer Protocol (HTTP/1.1) part 5: Range Requests",
http://datatracker.ietf.org/doc/draft-ietf-httpbis-p5-range/

For a detailed description of this sample, please see
http://blogs.msdn.com/b/webdev/archive/2012/11/23/asp-net-web-api-and-http-byte-range-support.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
