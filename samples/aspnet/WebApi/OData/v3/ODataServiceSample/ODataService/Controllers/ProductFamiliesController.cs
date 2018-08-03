using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Microsoft.Data.OData;
using ODataService.Models;

namespace ODataService.Controllers
{
    /// <summary>
    /// This controller is responsible for the ProductFamilies entity set.
    /// </summary>
    public class ProductFamiliesController : ODataController
    {
        private ProductsContext _db = new ProductsContext();

        /// <summary>
        /// Support for querying ProductFamilies
        /// </summary>
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Supported | AllowedQueryOptions.Format)]
        public IQueryable<ProductFamily> Get()
        {
            // if you need to secure this data, one approach would be
            // to apply a where clause before returning. This way any $filter etc, 
            // will be applied only after $filter
            return _db.ProductFamilies;
        }

        /// <summary>
        /// Support for getting a ProductFamily by key
        /// </summary>
        /// <param name="key"></param>
        [EnableQuery]
        public SingleResult<ProductFamily> Get(int key)
        {
            return SingleResult.Create(_db.ProductFamilies.Where(f => f.ID == key));
        }

        /// <summary>
        /// Support for creating a ProductFamily
        /// </summary>
        /// <param name="productFamily"></param>
        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> Post(ProductFamily productFamily)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.ProductFamilies.Add(productFamily);
            await _db.SaveChangesAsync();

            return Created(productFamily);
        }

        /// <summary>
        /// Support for replacing a ProductFamily
        /// </summary>
        public async Task<IHttpActionResult> Put([FromODataUri] int key, ProductFamily family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != family.ID)
            {
                return BadRequest();
            }

            _db.Entry(family).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.ProductFamilies.Any(p => p.ID == key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(family);
        }

        /// <summary>
        /// Support for patching a ProductFamily
        /// </summary>
        /// <param name="key"></param>
        /// <param name="productFamily"></param>
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> Patch(int key, Delta<ProductFamily> productFamily)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = await _db.ProductFamilies.FindAsync(key);
            if (update == null)
            {
                return NotFound();
            }

            productFamily.Patch(update);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.ProductFamilies.Any(p => p.ID == key))
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
        /// Support for deleting a ProductFamily
        /// </summary>
        /// <param name="key"></param>
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var productFamily = await _db.ProductFamilies.Where(p => p.ID == key).Include(p => p.Products).FirstOrDefaultAsync();
            if (productFamily == null)
            {
                return NotFound();
            }

            foreach (var product in productFamily.Products)
            {
                product.Family = null;
            }

            _db.ProductFamilies.Remove(productFamily);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Support for /ProductFamilies(1)/Products
        /// </summary>
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return _db.ProductFamilies.Where(pf => pf.ID == key).SelectMany(pf => pf.Products);
        }

        /// <summary>
        /// Support for POST /ProductFamiles(1)/Products
        /// </summary>
        [HttpPost]
        public async Task<IHttpActionResult> PostProducts([FromODataUri] int key, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var family = await _db.ProductFamilies.FindAsync(key);
            if (family == null)
            {
                return NotFound();
            }

            family.Products.Add(product);

            await _db.SaveChangesAsync();

            return Created(product);
        }

        /// <summary>
        /// Support ProductFamily.Products.Add(Product)
        /// </summary>
        [AcceptVerbs("POST", "PUT")]
        public async Task<IHttpActionResult> CreateLinkToProducts([FromODataUri] int key, [FromBody] Uri uri)
        {
            var family = await _db.ProductFamilies.FindAsync(key);
            if (family == null)
            {
                return Content(HttpStatusCode.NotFound, ODataErrors.EntityNotFound("ProductFamilies"));
            }

            var productId = Request.GetKeyValue<int>(uri);
            var product = await _db.Products.SingleOrDefaultAsync(p => p.ID == productId);
            if (product == null)
            {
                return Content(HttpStatusCode.NotFound, ODataErrors.EntityNotFound("Products"));
            }
            product.Family = family;

            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Support for /ProductFamilies(1)/Supplier
        /// </summary>
        [EnableQuery]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            return SingleResult.Create(_db.ProductFamilies.Where(pf => pf.ID == key).Select(pf => pf.Supplier));
        }

        /// <summary>
        /// Support ProductFamily.Supplier = Supplier
        /// </summary>
        [AcceptVerbs("POST", "PUT")]
        public async Task<IHttpActionResult> CreateLinkToSupplier([FromODataUri] int key, [FromBody] Uri uri)
        {
            var family = await _db.ProductFamilies.FindAsync(key);
            if (family == null)
            {
                return Content(HttpStatusCode.NotFound, ODataErrors.EntityNotFound("ProductFamilies"));
            }

            var supplierId = Request.GetKeyValue<int>(uri);
            var supplier = await _db.Suppliers.SingleOrDefaultAsync(p => p.ID == supplierId);
            if (supplier == null)
            {
                return Content(HttpStatusCode.NotFound, ODataErrors.EntityNotFound("Suppliers"));
            }
            family.Supplier = supplier;

            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Support for ProductFamily.Products.Delete(Product)
        /// 
        /// which uses this URL shape:
        ///     DELETE ~/ProductFamilies(id)/$links/Products(relatedId)
        /// </summary>
        public async Task<IHttpActionResult> DeleteLink([FromODataUri] int key, [FromODataUri] string relatedKey, string navigationProperty)
        {
            var family = await _db.ProductFamilies.FindAsync(key);
            if (family == null)
            {
                return Content(HttpStatusCode.NotFound, ODataErrors.EntityNotFound("ProductFamilies"));
            }
            switch (navigationProperty)
            {
                case "Products":
                    var productId = Convert.ToInt32(relatedKey);
                    var product = await _db.Products.SingleOrDefaultAsync(p => p.ID == productId);

                    if (product == null)
                    {
                        return Content(HttpStatusCode.NotFound, ODataErrors.EntityNotFound("Products"));
                    }
                    product.Family = null;
                    break;
                default:
                    return Content(HttpStatusCode.NotImplemented, ODataErrors.DeletingLinkNotSupported(navigationProperty));

            }
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Support for /ProductFamilies(1)/CreateProduct
        /// </summary>
        /// <param name="key">Bound key</param>
        /// <param name="parameters"></param>
        [HttpPost]
        public async Task<IHttpActionResult> CreateProduct([FromODataUri] int key, ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productFamily = await _db.ProductFamilies.FindAsync(key);
            var productName = parameters["Name"].ToString();

            var product = new Product
            {
                Name = productName,
                Family = productFamily,
                ReleaseDate = DateTime.Now,
                SupportedUntil = DateTime.Now.AddYears(10)
            };
            _db.Products.Add(product);

            await _db.SaveChangesAsync();

            return Ok(product.ID);
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