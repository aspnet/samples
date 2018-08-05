CustomAssemblyResolverSample
----------------------------

This sample illustrates how to modify ASP.NET Web API to support discovery of 
controllers loaded dynamically from a dynamically loaded controller library assembly.

The sample implements a custom IAssembliesResolver which takes the default 
implementation providing the default list of assemblies and then adds the 
ControllerLibrary assembly as well.
