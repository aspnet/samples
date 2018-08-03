BSON formatting sample
----------------------

This ASP.NET Web API sample illustrates how to use the new
BsonMediaTypeFormatter class. This class supports the application/bson media
type and BSON format.

To execute the sample:
1. Hit F5 to start the service (BSONSample project).
2. Right click on the Client project in Solution Explorer and select Debug then
   Start New Instance to start the client. Or run Client\bin\Debug\Client.exe
   from a command prompt.
3. After reviewing the output, hit ENTER in the client console window to exit
   the program.
4. If you wish to repeat the data exchanges, repeat steps 1 through 3.
5. Close the Internet Explorer window or IIS Express (using its taskbar icon)
   to end the demonstration.

For the most part, the service is unchanged from the default ASP.NET Web API
5.0 project with scaffolding of a simple model to generate an Entity Framework
controller.

The single formatter-related change to the service is found in
BSONSample\App_Start\WebApiConfig.cs. Please see the last few lines in that
file for details. Note the default formatters remain available. For example
clients may choose to send and receive JSON or XML media types.

The client exercises actions of the generated MyData controller. It sends and
receives all data using the application/bson media type and associated
formatter.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487