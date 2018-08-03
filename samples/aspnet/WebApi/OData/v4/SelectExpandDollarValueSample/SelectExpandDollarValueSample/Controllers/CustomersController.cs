using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using SelectExpandDollarValueSample.Model;

namespace SelectExpandDollarValueSample.Controllers
{
    public class CustomersController : ODataController
    {
        ShoppingContext context = new ShoppingContext();

        [EnableQuery(PageSize = 10, MaxExpansionDepth = 2)]
        public IHttpActionResult Get()
        {
            return Ok(context.Customers);
        }

        [EnableQuery(PageSize = 10, MaxExpansionDepth = 2)]
        public Task<IHttpActionResult> Get([FromODataUri] int key)
        {
            return Task.FromResult((IHttpActionResult)Ok(SingleResult.Create<Customer>(context.Customers.Where(
                customer => customer.Id == key))));
        }

        public async Task<IHttpActionResult> GetName(int key)
        {
            Customer customer = await context.Customers.FindAsync(key);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customer.Name);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
    }
}
