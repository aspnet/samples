ODataServiceSample
------------------

This sample illustrates how to create an OData service consisting of four entities and
three ApiControllers. The controllers provide various levels of functionality in terms
of the OData functionality they expose. In addition the OData service exposes a 
$metadata document which allows the data to the consumed by WCF Data Service clients 
and other clients that accept the $metadata format.

The data model contains three entities (Supplier, ProductFamily, and Product) connected 
in the following manner:

* Supplier
    -> Collection<ProductFamily>
    -> Address

* ProductFamily
    -> Supplier
    -> Collection<Product>

* Product
    -> ProductFamily

In addition, Supplier has a complex type property called Address.

The sample contains three sample controllers demonstrating various levels of 
exposing OData:

* The SupplierController exposes a subset of functionality including Query, Get by Key and 
Create, by handling these requests:

  GET /Suppliers
  GET /Suppliers(key)
  GET /Suppliers?$filter=..&$orderby=..&$top=..&$skip=..
  POST /Suppliers

* The ProductsController exposes GET, PUT, POST, DELETE, and PATCH by implementing an 
action for each of these operations directly.

* The ProductFamilesController leverages the EntitySetController base class which
exposes a useful pattern for implementing a rich OData service.

Furthermore, the ODataService exposes a Service Document (aka. $metadata document) that 
lists all the top-level entities so clients can discover them. This enables OData clients
to discover and consume OData Services exposed through ASP.NET Web APO.

Update (hongyes 2/18/2013)

1. Added $format support in the sample. The Extensions.FormatQueryMessageHandler class handles 
$format from query string and change accept header based on it. It accepts:
    * $format=xml => application/xml
    * $format=json => application/json
    * $format=atom => application/atom+xml

2. Added Extensions/NavigationRoutingConvention2 class to support POST, PUT and DELETE http 
method on navigation property. The default NavigationRoutingConvention only supports GET method.
The new convention demonstrates how to customize routing convention and how to register it into 
OData route.

For a detailed description of this sample, please see 
http://blogs.msdn.com/b/alexj/archive/2012/08/15/odata-support-in-asp-net-web-api.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
