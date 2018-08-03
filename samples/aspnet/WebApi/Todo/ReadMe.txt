Todo sample
----------------

This sample illustrates how to write multiple secure clients (browser, Windows Phone,
Windows Store) that consume the same backend Web API. 

Prerequisites:

- Windows 8.1
- Windows Phone 8 SDK (http://www.microsoft.com/en-us/download/details.aspx?id=35471)

Projects:

- Todo.Web: A web application project that contains the backend Web API OData service and the browser client.
- Todo.WindowsPhone: Windows Phone 8 client (requires the Windows Phone 8 SDK).
- Todo.WindowsStore: Windows Store client.
- WebApi.Client: Common portable client logic.
- Account.Client: Portable client for token acquisition and account management
- Todo.Client: Portable client for managing todo lists
- Todo.Tests: Test project that shows in-memory hosting of an OWIN pipeline using the Microsoft.Owin.Testing package.
- WebAuthenticationBroker: A browser control based implementation of the Web Authentication Broker for Windows Phone 8.

Running the sample:

You can run the Todo.Web project locally on your machine to host the Web API OData service and the browser client.
The Windows Phone 8 client and the Windows Store client are configured to target an existing deployment of the
Web application on Windows Azure (https://securetodo.azurewebsites.net) due to limitations in these client that
prevent them from accessing localhost. You can optionally deploy the Web application to your own Windows Azure 
Web Site and target the client applications at your own deployment (ex. for remote debugging, control of the 
database, etc.). You can sign up for a free trial of Windows Azure at
http://www.windowsazure.com/en-us/pricing/free-trial/.

To enable the Windows Store client to access your own deployment you must specify the following value in your app
settings: 
- windowsStoreRedirectUri=ms-app://s-1-15-2-452644135-994143696-4050500513-2520209098-3543204159-2432544503-3591636928/

To enable support for social logins you must first register your application with the corresponding login providers and
provide the following app settings in your web application (i.e. in web.config or through the Windows Azure Portal):
- microsoft_clientid=[Microsoft client ID]
- microsoft_clientsecret=[Microsoft client secret]
- facebook_appid=[Facebook app ID]
- facebook_appsecret=[Facebook app secret]
- twitter_consumerkey=[Twitter consumer key]
- twitter_consumersecret=[Twitter consumer secret]

For a detailed description of this sample, please see
http://go.microsoft.com/fwlink/?LinkId=356160

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487