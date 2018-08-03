PushContentControllerSample
---------------------------

This sample illustrates using the PushStreamContent to write a never-ending response
to a client. The PushContentController writes a little bit of data to the client every
second until the client disconnects.

The PushStreamContent class enables scenarios where a data producer wants to write directly
(either synchronously or asynchronously) using a stream. When the PushStreamContent is ready 
to accept data it calls out to an action with the output stream. The developer can then write 
to the stream for as long as necessary and close the stream when writing has completed. 
The PushStreamContent detects the closing of the stream and completes the underlying 
asynchronous Task for writing out the content. 

For a detailed description of this sample, please see 
http://blogs.msdn.com/b/henrikn/archive/2012/04/23/using-cookies-with-asp-net-web-api.aspx

In addition, check out these two for an HTML5 client using the EventSource API:
http://www.strathweb.com/2012/05/native-html5-push-notifications-with-asp-net-web-api-and-knockout-js/
and
http://techbrij.com/1029/real-time-chart-html5-push-sse-asp-net-web-api

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487