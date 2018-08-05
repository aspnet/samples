ControllerSpecificConfigSample
------------------------------

This ASP.NET Web API sample illustrates how to set up per-controller specific configuration.
It defines an attribute which derives from IControllerConfiguration which, when put on a 
controller, can define specific settings that only apply to that controller type.  

In this sample, we set up a PostMultipleParametersFromBody parameter binder which 
binds multiple parameters from body for a POST message for that controller type.

The SampleController uses this to provide special parameter binding for a POST request.

For a detailed description of this sample, please see
http://blogs.msdn.com/b/jmstall/archive/2012/05/11/per-controller-configuration-in-webapi.aspx
