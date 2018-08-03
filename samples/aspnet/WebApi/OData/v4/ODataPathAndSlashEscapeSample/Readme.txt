ODataPathAndSlashEscapeSample
------------------

String literals that include the URL encoded slash (%2F), and backslash(%5C) cause a 404 error when they are used in the OData resource paths.  
For example, string literals can be used in the OData resource paths as parameters of functions or key values of entity sets.  

~/Employees/Total.GetCount(Name='Name%2F') 
~/Employees(‘Name%5C’)  

When services receive such requests, the hosts will un-escape those escape sequences before passing them to the Web API runtime. 
This protects against attacks like the following:  http://www.contoso.com/..%2F..%2F/Windows/System32/cmd.exe?/c+dir+c:\ 
But, this causes the Web API OData stack to return a 404 error (Not Found). 

As a workaround you could override the Parse method of DefaultODataPathHandler to escape the slash and backslash in string literals 
before actually parsing them. This sample shows this apporach. 

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
