using Microsoft.AspNetCore.Mvc;

namespace MvcSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrictController : ControllerBase
    {
        // Will be called for accept: application/json
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<string> GetJson()
        {
            return "application/json";
        }

        // Will be called for accept: application/xml
        [HttpGet]
        [Produces("application/xml")]
        public ActionResult<string> GetXml()
        {
            return "application/xml";
        }

        // Other accept values will 404
    }
}