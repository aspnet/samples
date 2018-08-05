
using System.Web.Mvc;

namespace AttributeRoutingSample.Areas.Help.Controllers
{
    /// <summary>
    /// This controller uses attribute routing inside an Area.
    /// 
    /// The [RouteArea] attribute specifies a prefix for this area, and replaces the 'AreaConfig' 
    /// class that an area would typically require when using conventional routing. For URL matching
    /// purposes, the [RouteArea] attribute is similar to the [RoutePrefix] attribute.
    /// 
    /// The [Route] attribute defined on the controller uses a default value for the {action} parameter.
    /// In this case the default is 'Index', so this route can match patterns like both '/Help/About' and 
    /// '/Help/About/Index'. Note that the prefix specified by the [RouteArea] attribute is combined with
    /// the template.
    /// </summary>
    [RouteArea("Help")]
    [Route("About/{action=Index}")]
    public class AboutController : Controller
    {
        //
        // GET: /Help/About/
        // GET: /Help/About/Index
        public ActionResult Index()
        {
            return View();
        }
	}
}