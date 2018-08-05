AspNetRoutes sample
----------------------

In some applications you will want to hook up OWIN components in the Asp.Net route table
side by side with non-OWIN components.  This sample shows how to use the RouteCollection
extension methods MapOwinPath and MapOwinRoute provided by Microsoft.Owin.Host.SystemWeb.
