SelectExpandDollarValueSample
-----------------

This sample shows how to use $select, $expand and $value within Web API 2 OData.
$select allows the client to specify a subset of the properties from the entity
to be returned. $expand allows the client to specify a set of related entities 
to be returned with the queried feed or entity.

$value is a feature specific to OData.The process for using $select and $expand
in a regular Web API 2 is identical to the one described for OData, however, its
important to note that $select and $expand only work on the JSON formatter using
JSON.NET

For more information check:
http://blogs.msdn.com/b/webdev/archive/2013/07/05/introducing-select-and-expand-support-in-web-api-odata.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487