RoutingConstraints Sample
-------------------------

This sample shows how to use Attribute Routing and Constraints in ASP.NET Web API to dynamically filter
controllers by an 'api-version' HTTP header. When a route uses Constraints, each constraint has a chance
to prevent the route from matching a given request. In this sample, a custom RouteFactoryAttribute 
(VersionedRoute) adds a constraint to each attribute route.

There are two implementations of the Customer controller, each returning data of a slightly different 
structure. The Customer controllers both map to the same URL, and the exact controller is selected by 
routing depending on which constraint matches. This technique can be used to create separate versions of
an API with the same URL structure, while isolating consumers from breaking changes.

The custom constraint implementation (VersionConstraint) is implemented based on the value of 'api-version'
matching an integer value. The value of the allowed version for the constraint is provided by the VersionedRoute
attribute placed on each controller. When a request comes in, the header value of 'api-version' is matched
against the expected version. This example uses a header but a constraint implementation could use any criteria
to decided if the request is valid for the route.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487