using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.OData.NHibernate;
using System.Web.OData.Query;
using NHibernate;
using NHibernateQueryableSample.Models;

namespace NHibernateQueryableSample
{
    public class CustomersController : ApiController
    {
        // default query validation settings.
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        private ISession _db = CustomersSessionFactory.Instance.OpenSession();

        public IEnumerable<Customer> GetCustomers(ODataQueryOptions<Customer> queryOptions)
        {
            // validate the query.
            queryOptions.Validate(_validationSettings);

            // Apply the query.
            IQuery query = queryOptions.ApplyTo(_db);

            Console.WriteLine("Executing HQL:\t" + query);
            Console.WriteLine();

            return query.List<Customer>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _db.Dispose();
        }
    }
}
