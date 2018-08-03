using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using ExternalEdmModel.Model;

namespace ExternalEdmModel.Controllers
{
    public class OrdersController : ODataController
    {
        CustomersContext context = new CustomersContext();

        [EnableQuery(PageSize = 5)]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(context.Orders);
        }

        public async Task<IHttpActionResult> Get(int key)
        {
            Order order = await context.Orders.FindAsync(key);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
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
