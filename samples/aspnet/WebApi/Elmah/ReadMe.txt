ASP.NET Web API ELMAH Exception Logging Sample
-----------------------------------
This sample demonstrates custom exception logging and handling with Web API.
An ElmahExceptionLogger logs all exceptions for viewing through ELMAH. A
GenericTextExceptionHandler sends back static text any time an unhandled
exception occurs.

An application have multiple exception loggers, which allows through ELMAH as
well as other mecanisms at the same time.

The ElmahExceptionLogger is intended to be re-used as-is by any Web API
application using WebHost.

The GenericTextExceptionHandler is only used for demonstration purposes in this
sample application. Other exception handlers would likely use more information
from the context. For local debugging purposes, an exception handler can set
context.Result to null no force all exceptions to propogate to the host level
(such as showing a yellow screen with exception details on WebHost).

To see the sample, run the website and click each of the links on the page that
appears.

For more information on global error handling in Web API, please see
http://aspnetwebstack.codeplex.com/wikipage?title=Global%20Error%20Handling

For more information on ELMAH, please see
http://code.google.com/p/elmah/

To install ELMAH via NuGet, please see
http://www.nuget.org/packages?q=ELMAH

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487