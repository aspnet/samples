ODataQueryLimitationsSample
--------------------------------------------------------------------

This sample is provided as part of the ASP.NET Web Stack sample repository at 
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487

This sample shows how to use the query limitations feature in Web API for OData v4.0.
The sample covers:

1) Showing how to limit filtering and sorting in properties implicitly through attributes.
2) Showing how to limit expansion implicitly through attributes. It can also be done explictly
   on the model builder, but that's not demonstrated here.
3) Showing how to override filtering and sorting limitations through the model builder.
4) Showing the response when a client tries to sort on an unsortable property.
5) Showing the response when a client tries to filter on a non filterable property.
6) Showing the response when a client tries to filter on a non filterable property within a
   path.
7) Showing the response when a client tries to expand a non expandable property.
8) Showing the response when a client tries to filter on a valid set of properties.