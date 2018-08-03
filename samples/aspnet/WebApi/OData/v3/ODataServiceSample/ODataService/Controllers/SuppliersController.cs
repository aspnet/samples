using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using ODataService.Models;

namespace ODataService.Controllers
{
    /// <summary>
    /// This controller implements support for Suppliers EntitySet.
    /// It does not implement everything, it only supports Query, Get by Key and Create, 
    /// by handling these requests:
    /// 
    /// GET /Suppliers
    /// GET /Suppliers(key)
    /// GET /Suppliers?$filter=..&$orderby=..&$top=..&$skip=..
    /// POST /Suppliers
    /// </summary>
    public class SuppliersController : ODataController
    {
        ProductsContext _db = new ProductsContext();

        public IQueryable<Supplier> Get()
        {
            return _db.Suppliers;
        }

        [EnableQuery]
        public SingleResult<Supplier> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_db.Suppliers.Where(s => s.ID == key));
        }

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> Post(Supplier supplier)
        {
            supplier.ProductFamilies = null;

            Supplier addedSupplier = _db.Suppliers.Add(supplier);
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
