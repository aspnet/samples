ASP.NET Web API RouteConstraintsAndModelBindersSample by Implementing Matrix Parameters
---------------------------------------------------------------------------------------

This sample uses route constraints and the custom model binding as a way to support matrix parameters with Web API. 
The idea of matrix parameters (or matrix URIs) originates from the design at 
http://www.w3.org/DesignIssues/MatrixURIs.html. This sample implements the support with flexibility in route-matching 
and model-binding.

To run the sample, first start the server RouteConstraintsAndModelBindersSample.Server; then run the client 
RouteConstraintsAndModelBindersSample.Client.

Route constraints let you restrict how the parameters in the route template are matched. The syntax for specifying 
constraints in attribute routes is "{parameter:constraint}".

    [Route("users/{id:int}"]
    public User GetUserById(int id) { ... }

    [Route("users/{name}"]
    public User GetUserByName(string name) { ... }

Here, the first route will only be selected if the "id" segment of the URI is an integer. 
Otherwise, the second route will be chosen.

To customize a route constraint, implement the IHttpRouteConstraint interface and register it in WebApiConfig.cs.
When implementing IHttpRouteConstraint, you can specify whether the route will be matched according to contexts like 
the HTTP request, the route, the name of the parameter, the list of parameter values, etc.

For detailed information on attribute routing and route constraints, please visit 
http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2#constraints

A custom model binder gives you access to contexts like the HTTP request, the action description, and the raw values 
from the route data, so you can specify how to bind your type's value from the URI. To create a model binder, 
implement the IModelBinder interface. This interface defines a single method, BindModel, where you can set the value of
bindingContext.Model and return true if it succeeds.

    bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext);

For detailed information on parameter binding and model binding, please visit
http://www.asp.net/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api

In the folder RouteConstraintsAndModelBindersSample.Server\, please see

- Files under ModelBinders\ for how to set up model binders and attributes.
- App_Start\WebApiConfig.cs and RouteConstraints\SegmentPrefixConstraints.cs for how to customize route constraints.
- Controller\FruitsController.cs for how to use those route constraints and model binders.

This sample is provided as part of the ASP.NET Web Stack sample repository at 
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487


Introduction to Matrix Parameters
---------------------------------

Matrix parameters are strings like "size=medium;color=red,green" and "rate=good" in the following uri example 
http://example.com/apples;size=medium;color=red,green/washington;rate=good,

- are name/value pairs that can provide additional information selecting a resource.
- can appear in any path segment of a URI.
    - e.g. /apples;order=m;color=red/washington;zipcode=98052 
    - e.g. /apples/washington;color=red;zipcode=98052;order=m
- are separated from the hierarchical path segments of the URI using a semicolon ";".
- can have multiple values separated by a comma ",".
    - e.g. /apples;order=medium,large,small

Matrix parameters vs. Query parameters

- Query parameters always come at the end of the URI and pertain to the full resource you are referencing.
- Matrix parameters
    - are not resources but descriptive attributes of certain segments of the URI.
    - help reference a resource when it is difficult to represent within only one hierarchy.

## Features

You can configure attribute routing and model-binding attributes [MatrixParameter] and [SegmentPrefix] to support
matrix parameters.

Take the following relative path for example.

    bananas;order=m;color=red/washington;zipcode=98052 
    -------segment-----------|---------segment--------
    -------|-----------------|----------|-------------
    prefix    matrix param       prefix   matrix param

### 1. Flexible route-matching.

Route "{washington:SegmentPrefix}" will match a segment with a prefix "washington".
Route "{location}" will match a segment with any prefix.
Route "optional/{*CatchAll}" will match all segments after "optional/".

Please see route attributes on actions in the RouteConstraintsAndModelBindersSample.Server\FruitsController.cs 
and URIs in the RouteConstraintsAndModelBindersSample.Client\Program.cs for detailed examples. Please see 
RouteConstraintsAndModelBindersSample.Server\App_Start\WebApiConfig.cs for how to register the route constraint 
SegmentPrefixConstraint.

### 2. Flexible model-binding.

- You can specify that the matrix parameter values are bound 
    - from the segment with an exact prefix. e.g. [MatrixParameter("apples")] color.
    - from a route segment, e.g. [MatrixParameter("{Fruits}")] color.
    - from any segment of the URI, e.g. [MatrixParameter] color.
- You can specify that the parameter value is bound from the segment prefix, e.g. [SegmentPrefix] string fruits.

Please see model-binding attributes on parameters of actions in the 
RouteConstraintsAndModelBindersSample.Server\FruitsController.cs for detailed examples.
