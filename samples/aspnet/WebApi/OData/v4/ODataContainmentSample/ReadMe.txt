ODataContrainmentSample
------------------

This sample implements a very simple account management system, which can query, add accounts and do CRUD on the contained entities.

It illustrates how to use containment within an OData service. Such as:
1. Build the Edm Model that has containment navigation property
2. Expand containment navigation property (GET ~/Accounts?$expand=PayoutPI)
3. Query a containment navigation property directly (GET ~/Accounts(1)/PayinPIs)
4. Add a contained entity to a collection navigation property (POST ~/Accounts(1)/PayinPIs)
5. Update or add a nullable containment navigation property  (PUT ~/Accounts(1)/PayoutPI)
6. Delete a contained entity (DELETE ~/Accounts(1)/PayinPIs(1))
7. Call a function that is bound to a collection of contained entities (GET ~/Accounts(1)/PayinPIs/Namespace.GetCount())

It is a console application, just open the solution and hit F5 to run it.

This sample is provided as part of the ASP.NET Web Stack sample repository at 
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
