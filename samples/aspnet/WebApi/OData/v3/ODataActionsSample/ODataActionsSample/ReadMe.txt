This sample illustrates various ways to implement OData actions in ASP.NET Web API.

The sample implements a simple movie rental scenario. The application displays a list of movies,
and the user can check out or return a movie. You can also add a movie to the list. 

Actions:

Check out a movie:  /odata/Movies(1)/CheckOut
Return a movie:     /odata/Movies(1)/Return
Check out sevaral:  /odata/Movies/CheckOutMany
Add a movie:        /odata/CreateMovie

These actions demonstrate some of the of various options for OData actions:

* Always-bindable actions
* Transient (occasionally bindable) actions
* Actions bound to an entity
* Actions bound to an entity collection
* Non-bindable actions

