using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiAttributeRoutingSample.Models;

namespace WebApiAttributeRoutingSample.Controllers.Api
{
    /// <summary>
    /// This controller demonstrates default ordering of attribute routes.
    /// In general, as per the routing guidelines, more specific routes should be registered
    /// before generic ones. Attribute routing by default tries to do the best it can to order
    /// the routes according to it.
    /// 
    /// For the following controller, the routes are ordered like the following:
    /// 1. api/categories/all
    /// 2. api/categories/{id:int}
    /// 3. api/categories/{name}
    /// 
    /// </summary>
    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {
        private ShoppingContext db = new ShoppingContext();

        // GET api/categories/5
        [Route("{id:int}", Name = "GetCategoryById")]
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET api/categories/devices
        [Route("{name}", Name = "GetCategoryByName")]
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(string name)
        {
            Category category = await db.Categories.Where(c => c.Name == name).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET api/categories/all
        [Route("all")]
        public IQueryable<Category> GetCategories()
        {
            return db.Categories;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.Id == id) > 0;
        }
    }
}