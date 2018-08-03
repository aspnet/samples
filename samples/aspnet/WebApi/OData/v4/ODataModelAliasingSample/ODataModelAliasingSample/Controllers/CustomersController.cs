using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ODataModelAliasingSample.Model;

namespace ODataModelAliasingSample.Controllers
{
    public class CustomersController : ODataController
    {
        private static CustomersContext _context;

        public CustomersController()
        {
            _context = new CustomersContext();
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_context.Customers);
        }

        [EnableQuery]
        public IHttpActionResult GetOrders(int key)
        {
            CustomerDto customer = _context.Customers.Include(c => c.Purchases).SingleOrDefault(c => c.Id == key);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer.Purchases);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
