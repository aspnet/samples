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
    /// Here we demonstrate the following:
    /// - Decorate attribute route on each action. 
    /// - Escaping RoutePrefix by using '~/'
    /// - Multiple Route attributes on an action
    /// - UriPathExtensionMapping with attribute routing
    /// 
    /// NOTE:
    /// - RoutePrefix never creates a route in the route table, where as RouteAttribute does
    /// - This example is a verbose way of using attribute routing where each action is attributed explicitly. You can take a look
    ///   at the CustomersController & OrdersController for better ways of improving this controller.
    /// </summary>
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private ShoppingContext db = new ShoppingContext();

        // GET api/products
        [Route]
        // GET api/products.xml
        [Route("~/api/products.{ext}")]
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET api/products/5
        [Route("{id}", Name = "GetProductById")]
        // GET api/products/5.xml
        [Route("{id}.{ext}")]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT api/products/5
        [Route("{id}")]
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST api/products
        [Route]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        // DELETE api/products/5
        [Route("{id}")]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}