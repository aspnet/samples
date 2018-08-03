BingTranslateSample
-------------------

Sample illustrating using the Bing translator API from HttpClient. For details about the
Bing translator API, please see http://msdn.microsoft.com/en-us/library/ff512419.aspx

The API requires an OAuth token which we obtain by sending a request to the Azure token server 
each time we send a request to the translator service. The result from that request is fed 
into the request sent to the translation service itself.

Before you can run this sample you must obtain an application key from Azure Marketplace and 
fill in the information in the AccessTokenMessageHandler class, see 
http://msdn.microsoft.com/en-us/library/hh454950.aspx for details.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487