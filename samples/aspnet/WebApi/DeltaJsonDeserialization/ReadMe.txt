Delta Json.NET Deserialization Sample
-------------------------

This sample shows how to configure Json.NET to deserialize the Delta<T> type from Web API 2-2.1 OData. 
The Delta<T> type is typically used inside the OData protocol to allow partial updates for object using 
the HTTP PATCH method. This sample will demonstrate how to use the Delta<T> type with the popular Json.NET 
to support patching and other operations without using the OData protocol.

In this sample the PatchController exposes a data model with many different properties. Using the PATCH
method will allow a json blob with only some of these properties set to issue a partial update of an
object to the server.

To implement deserialization of this type, this sample configures Json.NET to use a custom JsonContract
for the Delta<T> type, by implementing a custom IContractResolver. See WebApiConfig.cs to see how the 
custom IContractResolver is configured. 

The contract created for Delta<T> will deserialize properties using the types and property names of the 
underlying type (SampleModel). The JsonProperty instances are copied from the SampleModel JsonContract and 
customized to work with a dynamic object. In particular, a custom IValueProvider is used to get and set 
values using the contract of DynamicObject, which Delta<T> inherits from.

The types JsonContract, IContractResolver, and IValueProvider are part of Json.NET's extensibility model,
you can find Json.NET's documentation at
http://james.newtonking.com/json/help/index.html


Note that the Delta<T> class is defined in the Web API OData package. Referencing the OData pacakge is 
required to use this class, but use of the OData entity model and protocol is not. To add this package 
to your project, run the following command at the NuGet Package Manager Console in Visual Studio:

Install-Package Microsoft.AspNet.WebApi.OData

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487