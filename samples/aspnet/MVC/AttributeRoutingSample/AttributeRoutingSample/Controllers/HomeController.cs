using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttributeRoutingSample.Controllers
{
    /// <summary>
    /// This controller uses attribute routing to map the Index action to several different URL paths.
    /// 
    /// The [RoutePrefix] attribute defines a prefix for all routes defined on the controller and on
    /// on actions. The [RoutePrefix] attribute on its own does not create any route entries, you must
    /// add [Route] attributes to create routes. 
    /// 
    /// In this example the [Route] attribute is used with no arguments to specify that '/Home' should 
    /// match the index action. 
    /// 
    /// The [Route] attribute is also used on the Index action with 'Index', this will be combined with 
    /// the 'Home' prefix to match '/Home/Index'
    /// 
    /// To override a prefix, specify the '~/' qualifier, and then the full path that should match that 
    /// route. In this example '~/' is used to specify a route that matches the root request path '/'.
    /// </summary>
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        // GET /
        // GET /Home
        // GET /Home/Index
        [Route("~/")]
        [Route]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}