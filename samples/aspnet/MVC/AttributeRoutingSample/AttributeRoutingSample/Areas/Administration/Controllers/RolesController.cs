using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttributeRoutingSample.Areas.Administration.Controllers
{
    /// <summary>
    /// NOTE:
    /// In this Administration area, we have the following setup:
    /// - UsersController uses conventional routes defined in AdminAreaRegistration.cs
    /// - RolesController uses attribute routing
    /// 
    /// For this kind of setup to work correctly with the rest of the application, MVC's attribute
    /// routing should be registered in an appropriate place. You can take a look at the Global.asax.cs file 
    /// for the route order that is recommended.
    /// </summary>
    [RouteArea("Administration", AreaPrefix = "Admin")]
    [RoutePrefix("Roles")]
    [Route("{action=Index}")]
    public class RolesController : Controller
    {
        // GET: /Admin/Roles/
        // GET: /Admin/Roles/Index
        public ActionResult Index()
        {
            return View();
        }
    }
}