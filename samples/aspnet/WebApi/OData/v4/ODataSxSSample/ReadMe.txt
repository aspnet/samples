ODataSxSSample
-------------------------

This sample illustrates how to implement OData services for OData V4 and OData V3 side by side. 
This sample solution combines two similar projects each of which uses OData V3 and OData V4 into 
a Web API application. It shows the scenario where users want to keep their existing V1 service
to use OData V3, but add a new service(ODataSxServiceV2) using the OData V4 package 
to the sampe Web API application. 

Actions:
/odata/Orders           : Access the V1 service without any version query string
/odata/Products(1)/?v=1 : Access the V1 service with the V1 version query string
/odata/$metadata        : Access the V1 metadata without any version query string
/odata/$metadata?v=1    : Access the V1 metadata with the V1 version query string
/odata/Orders(1)/?v=2   : Access the V2 service with the V2 version query string. 
                        : The V2 orders service uses the OData Attribute Routing 
						: when selecting actions. 
/odata/Products(1)/?v=2 : Access the V2 service with the V2 version query string. 
                        : The V2 products service in V2 rely on the traditional convention 
						: when selecting actions. 
/odata/$metadata?v=2    : Access the V2 metadata with the V2 version query string

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487

