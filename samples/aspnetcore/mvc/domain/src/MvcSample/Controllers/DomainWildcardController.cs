using DomainMatcherPolicy;
using Microsoft.AspNetCore.Mvc;

namespace MvcSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainWildcardController : ControllerBase
    {
        public ActionResult<string> GetAny()
        {
            return "*:*";
        }

        [Domain("*.0.0.1")]
        public ActionResult<string> GetWildcardDomain()
        {
            return "*.0.0.1:*";
        }

        [Domain("127.0.0.1")]
        public ActionResult<string> GetIP()
        {
            return "127.0.0.1:*";
        }

        [Domain("*.0.0.1:5000", "*.0.0.1:5001")]
        public ActionResult<string> GetWildcardDomainAndPorts()
        {
            return "127.0.0.1:5000,127.0.0.1:5001";
        }

        [Domain("contoso.com:*", "*.contoso.com:*")]
        public ActionResult<string> GetWildcardDomainAndBareDomain()
        {
            return "contoso.com:*,*.contoso.com:*";
        }
    }
}