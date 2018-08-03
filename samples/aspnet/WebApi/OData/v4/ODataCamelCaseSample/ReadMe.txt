ODataCamelCaseSample
------------------

C# always uses pascal case and javascript usually uses camel case. Camel Case feature will fill this gap: it generates EdM Model using Camel Case from the CLR Model using Pascal Case. 
Now only property names (both entity type properties and complex type properties) will be changed to camel case, but not for function/action names and their parameters, etc. 

This sample illustrates how to implement that. It is a console application, just open the solution and press F5 to run it.

This sample is provided as part of the ASP.NET Web Stack sample repository at 
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
