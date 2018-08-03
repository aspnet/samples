ODataQueryableSample
--------------------

This sample shows how to introduce OData queries in ASP.NET Web API using either the [EnableQuery] attribute
or by using the ODataQueryOptions action parameter which allows the action to manually inspect the query
before it is being executed. 

The CustomerController shows using [EnableQuery] attribute and the OrderController shows how to use the 
ODataQueryOptions parameter. The ResponseController is similar to the CustomerController but instead 
of the GET action returning IEnumerable<Customer> it returns an HttpResponseMessage. This allows us 
to add extra header fields, manipulate the status code, etc. while still using query functionality.

The sample illustrates queries using $orderby, $skip, $top, any(), all(), and $filter

For a detailed description of this sample, please see 
http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-api

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
