using DomainMatcherPolicy;
using Microsoft.AspNetCore.Mvc;

namespace MvcSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingleHostController : ControllerBase
    {
        public ActionResult<string> GetAny()
        {
            return "*:*";
        }

        [Domain("*:5000")]
        public ActionResult<string> GetAny5000()
        {
            return "*:5000";
        }

        [Domain("*:5001")]
        public ActionResult<string> GetAny5001()
        {
            return "*:5001";
        }

        [Domain("127.0.0.1:5000")]
        public ActionResult<string> GetIP5000()
        {
            return "127.0.0.1:5000";
        }

        [Domain("127.0.0.1:5001")]
        public ActionResult<string> GetIP5001()
        {
            return "127.0.0.1:5001";
        }

        [Domain("localhost:5003")]
        public ActionResult<string> GetLocalHost5003()
        {
            return "localhost:5003";
        }

        [Domain("127.0.0.1")]
        public ActionResult<string> GetIPAny()
        {
            return "127.0.0.1:*";
        }
    }
}