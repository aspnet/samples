using BatchSample.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace BatchSample.Controllers
{
    public class CustomersController : ApiController
    {
        CustomersContext context = new CustomersContext();

        [Queryable(PageSize = 10, MaxExpansionDepth = 2)]
        public IHttpActionResult Get()
        {
            return Ok(context.Customers);
        }

        public async Task<IHttpActionResult> Post([FromBody] Customer entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Customers.Add(entity);
            await context.SaveChangesAsync();
            return CreatedAtRoute("api", new { controller = "Customers" }, entity);
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody] Customer entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if (id != entity.Id)
            {
                return BadRequest("The key from the url must match the key of the entity in the body");
            }
            var originalCustomer = await context.Customers.FindAsync(id);
            if (originalCustomer == null)
            {
                return NotFound();
            }
            else
            {
                context.Entry(originalCustomer).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
            }
            return Content(HttpStatusCode.OK, entity);
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            Customer entity = await context.Customers.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                context.Customers.Remove(entity);
                await context.SaveChangesAsync();
                return StatusCode(HttpStatusCode.NoContent);
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