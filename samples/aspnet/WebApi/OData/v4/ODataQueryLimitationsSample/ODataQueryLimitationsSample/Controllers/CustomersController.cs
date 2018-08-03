using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ODataQueryLimitationsSample.Model;

namespace ODataQueryLimitationsSample.Controllers
{
    public class CustomersController : ODataController
    {
        private StoreContext _context = new StoreContext();

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_context.Customers);
        }

        protected override void Dispose(bool disposing)
        {
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
