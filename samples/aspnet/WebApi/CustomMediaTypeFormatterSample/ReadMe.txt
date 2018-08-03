CustomMediaTypeFormatterSample
------------------------------

This sample illustrates how to create a custom media type formatter using the 
BufferedMediaTypeFormatter base class for formatters which primarily are using 
synchronous read and write operations.

In addition to showing the media type formatter, the sample shows how to hook it up by
registering it as part of the HttpConfiguration for your application.

Note that it is also possible to use the MediaTypeFormatter base class directly for 
formatters which primarily use asynchronous read and write operations.

For a detailed description of this sample, please see
http://blogs.msdn.com/b/henrikn/archive/2012/04/23/using-cookies-with-asp-net-web-api.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487