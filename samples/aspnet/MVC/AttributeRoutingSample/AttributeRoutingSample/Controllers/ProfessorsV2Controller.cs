
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AttributeRoutingSample.Models;

namespace AttributeRoutingSample.Controllers
{
    /// <summary>
    /// This controller uses a custom RouteFactoryAttribute to generate a specialized route instance. 
    /// 
    /// This controller has some slight differences from the 'v1' Professors controller, it includes
    /// a list of courses and links right on the index page.
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
    [VersionedRoute("Professors/v{version}/{action=Index}/{id?}", 2, Name = "ProfessorsV2")]
    public class ProfessorsV2Controller : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: /Professors/v2/
        // GET: /Professors/v2/Index
        public async Task<ActionResult> Index()
        {
            return View(await db.Professors.Include(p => p.Courses).ToListAsync());
        }

        // GET: /Professors/v2/Details/5/
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
