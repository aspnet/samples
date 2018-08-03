NamespaceControllerSelector
---------------------------

This ASP.NET Web API sample shows how to support multiple API controllers with
the same name in different namespaces.

NOTE:
This sample only works for conventional routing. Please take a look at the 
following sample for attribute routing based versioning.
http://aspnet.codeplex.com/SourceControl/latest#Samples/WebApi/RoutingConstraintsSample/ReadMe.txt

For example, you could define two controllers named "ValuesController":

	MyApplication.V1.ValuesController
	MyApplication.V2.ValuesController

and then invoke the controllers with these URIs:

	/api/v1/values
	/api/v2/values

To make this work, the sample provides a custom implementation of the 
IHttpControllerSelector interface. The Web API pipeline uses this interface 
to select a controller during routing.

The custom IHttpControllerSelector looks for a "{namespace}" variable in the
route template. For example:

	"api/{namespace}/{controller}/{id}"

It tries to match this variable with the last segement of the controller's 
namespace. For example, if the namespace is "MyApplication.V1", it tries to 
match "V1" (case-insensitive).

When you run the console application, it sends an HTTP request to the "v1" URI
and another to the "v2" URI, and displays the results.