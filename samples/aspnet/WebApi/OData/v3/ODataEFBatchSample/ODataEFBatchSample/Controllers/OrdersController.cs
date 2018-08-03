using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using ODataEFBatchSample.Models;

namespace ODataEFBatchSample.Controllers
{
    public class OrdersController : ShoppingController
    {
        // GET odata/Orders
        [EnableQuery]
        public IQueryable<Order> GetOrders()
        {
            return Context.Orders;
        }

        // POST odata/Orders(5)/OrderLines
        public async Task<IHttpActionResult> PostToOrderLines(int key, OrderLine line)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await Context.Orders.FindAsync(key);

            order.OrderLines.Add(line);
            await Context.SaveChangesAsync();

            return Created(line);
        }
    }
}
