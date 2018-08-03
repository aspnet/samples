DynamicEdmModelCreation
-----------------------

This sample shows how to dynamically create an EDM model, and bind it into Web API OData pipeline.

Background:
By default, Web API OData supports a static EDM model per OData route.  In WebApiConfig.Register static method,
before calling config.Routes.MapODataServiceRoute, an EDM model is created, and then assigned as a parameter to the
MapODataServiceRoute method.
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Customer>("Customers");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
        }
    }

However, there are some other scenarios which require runtime EDM model binding, e.g.:
1. Multi-tenant OData service: One-model per tenant
2. Hundreds of models, want to delay load as many as possible in WebApiConfig
3. Per request model

This sample describes how to create a per-request EDM model, which can be used as a foundation for scenario 1 and 2:
In request Uri, there is a path segment between route prefix and entity set, e.g. mydatasource in 
http://servername/odata/mydatasource/Products. Based on datasource, the service could build EDM model on the fly,
and then use an untyped controller to handle the request.
The steps are:
1. Create a customized ODataPathRouteConstraint, which allows to set EdmModel property, before Match is called.
2. Create a customized ODataRoute to override GetVirtualPath logic, and generate OData links correctly.
3. Create a customized MapODataServiceRoute that takes a Func<HttpRequestMessage, IEdmModel> instead of an IEdmModel.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
