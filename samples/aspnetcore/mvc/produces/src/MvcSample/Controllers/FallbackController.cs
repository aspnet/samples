using Microsoft.AspNetCore.Mvc;

namespace MvcSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FallbackController : ControllerBase
    {
        // Will be called for accept: application/hal+json
        [HttpGet]
        [Produces("application/hal+json")]
        public ActionResult<string> GetJson()
        {
            return "application/hal+json";
        }

        // Will be called for accept: application/hal+xml
        [HttpGet]
        [Produces("application/hal+xml")]
        public ActionResult<string> GetXml()
        {
            return "application/hal+xml";
        }

        // Will be called as a fallback
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "*/*";
        }
    }
}