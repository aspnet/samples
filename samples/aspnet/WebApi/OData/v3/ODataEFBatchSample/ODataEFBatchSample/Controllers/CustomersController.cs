using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using ODataEFBatchSample.Models;

namespace ODataEFBatchSample.Controllers
{
    public class CustomersController : ShoppingController
    {
        // POST odata/Customers
        public async Task<IHttpActionResult> Post(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Context.Customers.Add(customer);
            await Context.SaveChangesAsync();

            return Created(customer);
        }

        // POST odata/Customers(5)/Orders
        public async Task<IHttpActionResult> PostToOrders(int key, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await Context.Customers.FindAsync(key);

            customer.Orders.Add(order);
            await Context.SaveChangesAsync();

            return Created(order);
        }

        // DELETE odata/Customers(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Customer customer = await Context.Customers.FindAsync(key);
            if (customer == null)
            {
                return NotFound();
            }

            Context.Customers.Remove(customer);
            await Context.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
