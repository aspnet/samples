ODataAuthorizationQueryValidatorSample
--------------------------------------------------------------------

This sample shows how to extend the query capabilities of OData in order to
perform authorization on $expand.

The sample covers:

1) Extending the EnableQueryAttribute to perform extra validations.
2) Creating an annotation to define which roles can expand a given
   navigation source or entity type.
3) Extending the EnableQueryAttribute to create the IEdmModel in query
   composition mode and annotate it for non Web API OData scenarios.