using System;
using System.Web.Http;

namespace Elmah.Server.Controllers
{
    public class ConstructorExceptionController : ApiController
    {
        public ConstructorExceptionController()
        {
            throw new InvalidOperationException("This exception was thrown in a controller constructor.");
        }

        // GET /constructor
        [Route("constructor")]
        public void Get()
        {
        }
    }
}