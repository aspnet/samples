using DomainMatcherPolicy;
using Microsoft.AspNetCore.Mvc;

namespace MvcSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionalController : ControllerBase
    {
        [Domain("*:5000", "*:5001")]
        public ActionResult<string> GetAny5000And5001()
        {
            return "*:5000,*:5001";
        }

        [Domain("*:5000")]
        public ActionResult<string> GetIP5000()
        {
            return "*:5000";
        }

        [Domain("*.0.0.1:5002")]
        public ActionResult<string> GetWildcardDomain()
        {
            return "*.0.0.1:5002";
        }

        [Domain("*.0.1:5002")]
        public ActionResult<string> GetAnotherWildcardDomain()
        {
            return "*.0.1:5002";
        }

        [Domain("*.localhost:5003")]
        public ActionResult<string> GetLocalHostWildcardDomain()
        {
            return "*.localhost";
        }
    }
}