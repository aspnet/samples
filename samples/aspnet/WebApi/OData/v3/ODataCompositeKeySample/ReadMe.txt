ODataCompositeKeySample
------------------

This sample illustrates how to create an OData service consisting of entity with 
composite key and how to provide basic CRUD functionalities on it.

The data model contains one entity (Person):

* Person
	-> FirstName (Key)
	-> LastName (Key)
	-> Age

The sample contains one People controller demostrating CRUD functionalities on Person.
The controller inherits from ODataController, which has more flexibility than 
EntitySetController, in order to handle multiple key parameters in actions. 
It can handle these requests:

	GET /People
	GET /People(FirstName='Kate',LastName='Jones')
	POST /People
	PATCH /People(FirstName='Kate',LastName='Jones')
	PUT /People(FirstName='Kate',LastName='Jones')
	DELETE /People(FirstName='Kate',LastName='Jones')

In addition, the sample contains a custom routing convention (CompositeKeyRoutingConvention) 
to support dispatching keys to action parameters. The convention is inherited from 
EntityRoutingConvention, which is the default convention to handle entity keys. It can 
convert the composite key from URI representation:

	FirstName='Kate',LastName='Jones'

to route values:

	FirstName: 'Kate'
	LastName: 'Jones'

And the model binder attribute [FromODataUri] will convert the OData URI literal to CLR type.
So you will get strong typed parameter in action:

public Person Get([FromODataUri] string firstName, [FromODataUri] string lastName)

For a detailed description of this sample, please see 
http://blogs.msdn.com/b/hongyes/archive/2013/02/06/asp-net-web-api-odata-support-composite-key.aspx

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
