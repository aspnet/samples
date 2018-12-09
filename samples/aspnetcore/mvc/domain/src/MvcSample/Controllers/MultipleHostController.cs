using DomainMatcherPolicy;
using Microsoft.AspNetCore.Mvc;

namespace MvcSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultipleHostController : ControllerBase
    {
        public ActionResult<string> GetAny()
        {
            return "*:*";
        }

        [Domain("*:5000", "*:5001", "127.0.0.1")]
        public ActionResult<string> GetAny5000And5001OrIP()
        {
            return "*:5000,*:5001,127.0.0.1:*";
        }

        [Domain("127.0.0.1:5000", "127.0.0.1:5001")]
        public ActionResult<string> GetIP5000()
        {
            return "127.0.0.1:5000,127.0.0.1:5001";
        }
    }
}