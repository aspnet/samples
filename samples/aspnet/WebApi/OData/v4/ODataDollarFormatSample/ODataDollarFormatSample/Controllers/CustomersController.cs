using System.Web.Http;
using System.Web.OData;
using ODataDollarFormatSample.Model;

namespace ODataDollarFormatSample.Controllers
{
    public class CustomersController : ODataController
    {
        public CustomersContext _context = new CustomersContext();

        [EnableQuery(PageSize = 2)]
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
