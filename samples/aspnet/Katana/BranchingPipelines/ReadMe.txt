BranchingPipelines sample
----------------------

OWIN request processing pipelines do not need to be linear, they can be branched
to process requests in different ways.  This sample shows how to construct a
branching pipeline based on request paths or other request data such as headers.
These components are available in the Microsoft.Owin.Mapping nuget package.
