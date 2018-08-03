ODataServiceSample
------------------

This sample illustrates how to create an OData service consisting of four OData Controllers.
The controllers provide various levels of functionality in terms of the OData functionality 
they expose. In addition the OData service exposes a $metadata document which allows the data 
to the consumed by OData Client for .NET clients and other clients that accept the $metadata format.

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

* The controllers leverages the ODataController base class which
exposes a useful pattern for implementing a rich OData service.

Furthermore, the ODataService exposes a Service Document (aka. $metadata document) that 
lists all the top-level entities so clients can discover them. This enables OData clients
to discover and consume OData Services exposed through ASP.NET Web API.

For a detailed description of this sample, please see 
http://www.asp.net/web-api/overview/odata-support-in-aspnet-web-api

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
