using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using ODataService.Models;

namespace ODataService.Controllers
{
    /// <summary>
    /// This controller implements everything the OData Web API integration enables by hand.
    /// </summary>
    public class ProductsController : ODataController
    {
        // this example uses EntityFramework CodeFirst
        ProductsContext _db = new ProductsContext();

        /// <summary>
        /// Adds support for getting products, for example:
        /// 
        /// GET /Products
        /// GET /Products?$filter=Name eq 'Windows 95'
        /// GET /Products?
        /// 
        /// <remarks>
        /// Support for $filter, $orderby, $top and $skip is provided by the [EnableQuery] attribute.
        /// </remarks>
        /// </summary>
        /// <returns>An IQueryable with all the products you want it to be possible for clients to reach.</returns>
        [EnableQuery]
        public IQueryable<Product> Get()
        {
            // If you have any security filters you should apply them before returning then from this method.
            return _db.Products;
        }

        /// <summary>
        /// Adds support for getting a product by key, for example:
        /// 
        /// GET /Products(1)
        /// </summary>
        /// <param name="key">The key of the Product required</param>
        /// <returns>The Product</returns>
        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Products.Where(p => p.ID == key));
        }

        /// <summary>
        /// Support for creating products
        /// </summary>
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Created(product);
        }

        /// <summary>
        /// Support for updating products
        /// </summary>
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Product update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.ID)
            {
                return BadRequest();
            }

            _db.Entry(update).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Products.Any(p => p.ID == key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(update);
        }

        /// <summary>
        /// Support for partial updates of products
        /// </summary>
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _db.Products.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            product.Patch(entity);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.Products.Any(p => p.ID == key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        /// <summary>
        /// Support for deleting products by key.
        /// </summary>
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await _db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds support for getting a ProductFamily from a Product, for example:
        /// 
        /// GET /Products(11)/Family
        /// </summary>
        /// <param name="key">The id of the Product</param>
        /// <returns>The related ProductFamily</returns>
        public async Task<IHttpActionResult> GetFamily([FromODataUri] int key)
        {
            var family = await _db.Products.Where(p => p.ID == key).Select(p => p.Family).SingleOrDefaultAsync();
            return Ok(family);
        }

        /// <summary>
        /// Support for creating links between entities in this entity set and other entities
        /// using the specified navigation property.
        /// </summary>
        /// <remarks>
        /// In this example Product only has a Product.Family relationship, which is a singleton, soon only PUT
        /// support is required, if there was a Product.Orders relationship - a collection - then this would need 
        /// to respond to POST requests too.
        /// </remarks>
        /// <param name="key">The key of the Entity in this EntitySet</param>
        /// <param name="navigationProperty">The navigation property of the Entity in this EntitySet that should be modified</param>
        /// <param name="link">The url to the other entity that should be related via the navigationProperty</param>
        [AcceptVerbs("POST", "PUT")]
        public async Task<IHttpActionResult> CreateLink([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
        {
            var product = await _db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            switch (navigationProperty)
            {
                case "Family":
                    // The utility method uses routing (ODataRoutes.GetById should match) to get the value of {id} parameter 
                    // which is the id of the ProductFamily.
                    var relatedKey = Request.GetKeyValue<int>(link);
                    var family = await _db.ProductFamilies.SingleOrDefaultAsync(f => f.ID == relatedKey);
                    product.Family = family;
                    break;

                default:

                    return Content(HttpStatusCode.NotImplemented, ODataErrors.CreatingLinkNotSupported(navigationProperty));
            }
            await _db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Support for removing links between resources
        /// </summary>
        /// <param name="key">The key of the entity with the navigation property</param>
        /// <param name="link">The url to the other entity that should no longer be linked to the entity via the navigation property</param>
        public async Task<IHttpActionResult> DeleteLinkToFamily([FromODataUri] int key, [FromBody] Uri link)
        {
            var product = await _db.Products.FindAsync(key);

            if (product == null)
            {
                return NotFound();
            }

            product.Family = null;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.Dispose(disposing);
        }
    }
}
