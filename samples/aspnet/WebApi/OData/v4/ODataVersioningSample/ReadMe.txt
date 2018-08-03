ODataVersioningSample
------------------

This sample illustrates how to create multiple versions of OData service in same 
application and how to support versioning by route, query string and header.

The data model is separated by DB models and view models in this sample. This model 
gives a good isolation between different OData versions. Here is the folder structure 
for this model:

	* V1
		-> Controllers
		-> ViewModels
	* V2
		-> Controllers
		-> ViewModels
	* Models 
		-> (DB models)

It works best if you have big data model changes between different versions and you 
don't want to impact any old client code. However, it increases the complexity that 
you need to maintain duplicate code between versions.

You may have other simpler solutions for versioning scenarios, which are not demonstrated in 
this sample. However, you can still reuse the code in it.

Scenario A: You don't have model changes but need to dispatch to different controllers for 
different versions.
	* In this scenario, you can remove the ViewModels under versioning folder.
	* You don't need to create two edm models. Just reuse one when mapping odata route.
	* You can still keep the routing and controller selector code in this sample.

Scenario B: You have simple model changes like add new property but you don't want to create 
two different models
	* You can customize EDM model by model builder to have different models, but still mapping 
	to same CLR type
	* Remove ViewModels from versiong folder.
	* Keep routing and controller selector code if you want to dispatch to versioning controller.

In this particular sample, the versioning scenario to add two new entity types, add new navigation 
property, change entity key type in V2.

	* Product (V1 entity TYPE)
		-> ID (Change from int to long in V2)
		-> Name
		-> ReleaseDate
		-> SupportUntil
		-> Family (V2 property that navigates to ProductFamily entity type)
	* ProductFamily (V2 entity type)
	* Supplier (V2 entity type)

In the sample, there are 3 pairs of routes for demonstrating different versioning methods.

	1. Versioning by route
	It can handle request by route prefix: /versionbyroute/v1 and /versionbyroute/v2.

	2. Versioning by query string
	It can handle request by query string: /versionbyquery?v=1 and /versionbyquery?v=2.

	3. Versioning by header
	It can handle request /versionbyheader with different http headers:
		v: 1
	and
		v: 2

#1 is done by default MapODataRoute extension method by matching route prefix. 
#2 and #3 is done by custom route constraint: ODataVersionRouteConstraint. It inherits from 
ODataPathRouteConstraint and performs query string and header check before entering into default 
odata route match. If nothing is matched, the routing code will fall back to check the next route.
Once a route is matched, the base ODataPathRouteConstraint will set the specified EDM model in the 
request, so that the request will stick to a specific version. This is crucial as the edm model 
will be used in all places in OData pipeline include building OData path, query composition, 
$metadata and service document.

After the right route with specific version of EDM model is picked, we still need a controller 
selector to select the controller with correct version. In order to simplify the scenario,
the sample just supports to dispatch controller with version suffix. 

	* ProductV1Controller
	* ProductV2Controller

The ODataVersionControllerSelector maps route name with suffix name. So it means any request:
	
	* matching /versionbyroute/v1 route will be dispatched to controller whose name ending with V1;
	* matching /versionbyquery?v=2 route will be dispatched to controller whose name ending with V2;
	...

If there is no versioning controller found, it will fall back to default controller selector logic.
You may implement your own controller selector to support more advanced scenario like selecting
controller by namespace.

Finally, the sample also provides an implementation of client code with WCF data service client.
It demonstrates how to specify version in route, query string and header.

The sample uses AutoMapper to simplify the mapping code between domain model and view model. For
more information about AutoMapper, please see
http://automapper.org/

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487

