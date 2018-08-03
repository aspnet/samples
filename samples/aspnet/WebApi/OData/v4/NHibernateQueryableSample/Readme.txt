NHibernateQueryableSample
-------------------------

This sample shows how to apply ODataQueryOptions<T> to a custom backend that is not IQueryable.

The CustomersController shows applying ODataQueryOptions<T> to a NHibernate.ISession object.
The sample illustrates queries using $orderby, $skip, $top, any(), all(), and $filter

The project System.Web.Http.OData.NHibernate contains code that translates an ODataQueryOptions<T> to HQL. The 
class ODataQueryOptionExtensions contains code for translating $skip, $top. The class NHibernateOrderByBinder 
contains code for translating $orderby to HQL's orderby clause. The class NHibernateFilterBindercontains code 
for translating $filter to the corresponding HQL where clause.

For more information about NHibernate, see http://nhforge.org/

This sample is provided as part of the ASP.NET Web Stack sample repository at
http://aspnet.codeplex.com/

For more information about the samples, please see
http://go.microsoft.com/fwlink/?LinkId=261487
