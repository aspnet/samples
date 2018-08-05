using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttributeRoutingSample.Areas.Administration.Controllers
{
    /// <summary>
    /// This controller does not use attribute routing and uses the conventional
    /// route defined in AdministrationAreaRegistration.cs
    /// </summary>
    public class UsersController : Controller
    {
        //
        // GET: /Admin/Users/
        public ActionResult Index()
        {
            return View();
        }
	}
}