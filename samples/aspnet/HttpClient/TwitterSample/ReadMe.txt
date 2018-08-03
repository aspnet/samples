TwitterSample
-------------

This sample illustrates how to write a simple twitter client using HttpClient. The sample uses a 
HttpMessageHandler to insert the appropriate OAuth authentication information into the outgoing
HttpRequestMessage. The result from twitter is read using JSON.NET as a JToken.

Before you can run this sample you must obtain an application key from twitter, and 
fill in the information in the OAuthMessageHandler class, see 
http://dev.twitter.com/ for details.

For a detailed description of this sample, please see
http://blogs.msdn.com/b/henrikn/archive/2012/02/16/extending-httpclient-with-oauth-to-access-twitter.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487