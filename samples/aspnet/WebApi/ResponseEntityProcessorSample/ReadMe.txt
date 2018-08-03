ResponseEntityProcessorSample
-----------------------------

This sample illustrates how to copy a response entity (i.e. an HTTP response body) to a local file 
before it is transmitted to the client and perform additional processing on that file asynchronously. 
It does so by hooking in a HttpMessageHandler that wraps the response entity with one that both
writes itself to the output as normal and to a local file.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487