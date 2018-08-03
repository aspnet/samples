
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using AttributeRoutingSample.Models;

namespace AttributeRoutingSample.Controllers
{
    /// <summary>
    /// This controller uses routes defined on actions with a route defined at the controller-level.
    /// 
    /// Routes can be defined at the controller level by placing the [Route] attribute on the controller
    /// class. Any route specified in this way should include {action} as a parameter, so that it's clear
    /// which action is to be invoked. If a request matches multiple actions, it will return an HTTP 404.
    /// 
    /// In this example, the [Route] attribute on the controller class will match a URL pattern like 
    /// '/Students/Edit/5'. The 'Students' prefix is prepended to the template. The {id:int} parameter 
    /// forces the required id parameter to have an integer value. This route can only match the Edit, 
    /// Details, and Delete actions on this controller.
    /// 
    /// The [Route] attribute defined on the controller class also provides a Name so that it can used to
    /// generate URLs via the Html.RouteLink method. In attribute routing, actions and controllers generally
    /// are configured to match multiple URL patterns using the Name property allows you to have control
    /// over which routes are used to generate links.
    /// 
    /// 
    /// Routes defined on actions override any routes that are defined on the controller. For instance,
    /// the Index action has two [Route] attribute which will match '/Students' and '/Students/Index'.
    /// These routes replace the route defined at the controller level. '/Students/Index/5' will not match 
    /// this action. Note that the [RoutePrefix] defined at the controller level still applies.
    /// </summary>
    [RoutePrefix("Students")]
    [Route("{action}/{id:int}", Name = "StudentsRoute")]
    public class StudentsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: /Students/
        // GET: /Students/Index
        [Route(Name = "GetAllStudentsView")]
        [Route("Index")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Students.ToListAsync());
        }

        // GET: /Students/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Students/Create
        [Route("Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,MiddleName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: /Students/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,MiddleName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: /Students/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Student student = await db.Students.FindAsync(id);
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
