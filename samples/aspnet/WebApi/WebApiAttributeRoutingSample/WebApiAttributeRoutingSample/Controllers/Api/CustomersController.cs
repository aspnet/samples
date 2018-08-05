using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiAttributeRoutingSample.Models;

namespace WebApiAttributeRoutingSample.Controllers.Api
{
    /// <summary>
    /// This controller demonstrates mixing conventional and attribute routing together in a single controller.
    /// Here 'GetOrdersOfCustomer' is the only action which uses attribute routing, where as the remaining actions
    /// are accessible through conventional route.
    /// 
    /// NOTE:
    /// - Conventional routes can never access actions/controllers which are decorated with route attributes.
    /// </summary>
    public class CustomersController : ApiController
    {
        private ShoppingContext db = new ShoppingContext();

        // GET api/customers
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        // GET api/customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT api/customers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.Id)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/customers
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);
        }

        // DELETE api/customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> DeleteCustomer(int id)
        {
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        [Route("api/customers/{id}/orders")]
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrdersOfCustomer(int id)
        {
            Customer customer = await db.Customers.Include(c => c.Orders).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer.Orders);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}