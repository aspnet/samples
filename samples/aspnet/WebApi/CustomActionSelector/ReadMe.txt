RoutingConstraints Sample
-------------------------

This sample shows how to use a custom Action Selector in ASP.NET Web API to dynamically chose an action for each 
request. When a controller is attributed with a class implementing IControllerConfiguration, the attribute class
can customize controller-level services to configure the desired behavior. In this sample the 
BetaTestingBehaviorAttribute implements A/B testing when the 'x-beta-tester' header is present and set to true
by randomly sending a portion of users to a 'beta' version of each action.

The Store controller implements A/B testing for the Contoso Company's new aggressive in-your-face marketing 
approach. Users that opt in to the beta program will randomly retrieve a different set of product descriptions
based on the time of the request.

The custom action selector determines based on these criteria whether or not a user should be shown the 'beta'
version of an action. To get the 'beta' action, the action selector looks for the presence of the 
'HasBetaActionAttribute' on the action matched by the default action selector. If this attribute is specified,
it can be used to identify an alternative method to invoke as the action method. A property on the 
ActionDescriptor can identify whether or not the action that was invoked was the standard action or the 'beta'
version.

Finally, the BetaTestingBehaviorAttribute implements an action filter to record a response header tracking
whether the standard action or the 'beta' version was invoked.

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487