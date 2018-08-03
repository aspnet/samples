using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using ODataDollarCountSample.Model;

namespace ODataDollarCountSample.Controllers
{
    public class CustomersController : ODataController
    {
        private IList<Customer> customers = new List<Customer>()
            {
                new Customer
                {
                    Id = 1,
                    Name = "Name1",
                    Emails = new List<string> { "a@microsoft.com", "a@live.com", "a@hotmail.com" },
                    ShipAddresses = new List<Address> 
                    { 
                        new Address{ City="Beijing", Street="Chang'an" },
                        new Address{ City="Shanghai", Street="Renmin" }
                    },
                    AvailableDays = new List<Day> { Day.Saturday, Day.Sunday },
                }
            };

        [EnableQuery(PageSize = 2)]
        public IHttpActionResult Get()
        {
            return Ok(customers);
        }

        // This actions matches both ~/Customers({key})/Emails and ~/Customers({key})/Emails/$count
        [EnableQuery]
        public IHttpActionResult GetEmails(int key)
        {
            Customer customer = customers.SingleOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer.Emails);
        }

        [HttpGet]
        [ODataRoute("Customers({key})/ShipAddresses/$count")]
        public IHttpActionResult GetShipAddressesCount(int key, ODataQueryOptions<Address> options)
        {
            Customer customer = customers.SingleOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }

            IQueryable<Address> eligibleAddresses = customer.ShipAddresses.AsQueryable();
            if (options.Filter != null)
            {
                eligibleAddresses = options.Filter.ApplyTo(eligibleAddresses, new ODataQuerySettings()).Cast<Address>();
            }
            return Ok(eligibleAddresses.Count());
        }

        // This actions matches both ~/Customers({key})/AvailableDays but not for ~/Customers({key})/Emails/$count
        // because the property AvailableDays is decorated with [NotCountable]
        [EnableQuery]
        public IHttpActionResult GetAvailableDays(int key)
        {
            Customer customer = customers.SingleOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer.AvailableDays);
        }
    }
}
