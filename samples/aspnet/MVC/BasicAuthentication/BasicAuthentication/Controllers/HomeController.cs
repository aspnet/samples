using System.Security.Claims;
using System.Web.Mvc;
using BasicAuthentication.Filters;
using BasicAuthentication.Models.Home;

namespace BasicAuthentication.Controllers
{
    [IdentityBasicAuthentication] // Enable authentication via an ASP.NET Identity user name and password
    [Authorize] // Require some form of authentication
    public class HomeController : Controller
    {
        [Route]
        public ActionResult Index()
        {
            IndexModel model = new IndexModel
            {
                UserName = User.Identity.Name
            };

            ClaimsIdentity identity = User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                model.Claims = identity.Claims;
            }

            return View(model);
        }
    }
}
