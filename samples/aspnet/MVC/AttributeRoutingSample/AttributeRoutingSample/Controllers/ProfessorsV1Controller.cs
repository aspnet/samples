
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AttributeRoutingSample.Models;

namespace AttributeRoutingSample.Controllers
{
    /// <summary>
    /// This controller uses a custom class derived from [RouteFactory] attribute to generate a specialized 
    /// route instance. 
    /// 
    /// This controller has some slight differences from the 'v2' Professors controller, it does not
    /// include a list of courses on the index page.
    /// 
    /// The action to be invoked on this controller is determined by the {action} parameter, which has a
    /// default of 'Index'. The {id} parameter is optional, so that a single route can match both Index
    /// and Details (which requires an id). Above each action are examples of request paths that would
    /// reach that action.
    /// 
    /// In this case the custom route implements versioning by looking at the parameters matched by the
    /// route template, but a variety of criteria could be used, see VersionedRouteAttribute.cs and 
    /// VersionedRouteConstraint.cs.
    /// </summary>
    [VersionedRoute("Professors/v{version}/{action=Index}/{id?}", 1, Name = "ProfessorsV1")]
    public class ProfessorsV1Controller : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: /Professors/v1/
        // GET: /Professors/v1/Index
        public async Task<ActionResult> Index()
        {
            return View(await db.Professors.ToListAsync());
        }

        // GET: /Professors/v1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = await db.Professors.Include(p => p.Courses).SingleAsync(p => p.Id == id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
