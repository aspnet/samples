HttpMessageHandlerPipelineSample
--------------------------------

This sample illustrates how to wire up HttpMessageHandlers on both client and server side
as part of either HttpClient or ASP.NET Web API.

In the sample, the same handler is used on both client and server side. While it is rare
that the exact same handler can run in both places, the object model is the same on 
client and server side.

For a detailed description of this sample, please see
http://blogs.msdn.com/b/henrikn/archive/2012/08/07/httpclient-httpclienthandler-and-httpwebrequesthandler.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487