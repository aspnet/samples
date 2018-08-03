CustomParameterBindingSample
----------------------------

This ASP.NET Web API sample illustrates how to customize the parameter binding process which is 
the process that determines how information from a request is bound to action parameters.

In this sample, the Home controller has four actions: 

1. BindPrincipal shows how to bind an IPrincipal parameter from a custom generic principal, 
   not from an HTTP GET message;

2. BindCustomComplexTypeFromUriOrBody shows how to bind a Complex Type parameter which could 
   come from either the message body or request Uri of an HTTP POST message;

3. BindCustomComplexTypeFromUriWithRenamedProperty shows how to bind a Complex Type parameter 
   with a renamed property which comes from request Uri of an HTTP POST message; 

4. PostMultipleParametersFromBody shows how to bind multiple parameters from body for a POST message;

For a detailed description of this sample, please see
http://blogs.msdn.com/b/hongmeig1/archive/2012/09/28/how-to-customize-parameter-binding.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487