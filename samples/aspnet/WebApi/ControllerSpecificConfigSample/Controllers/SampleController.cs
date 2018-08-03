using System;
using System.Net.Http;
using System.Web.Http;

namespace ControllerSpecificConfigSample.Controllers
{
    /// <summary>
    /// Sample controller with controller specific configuration in the form of
    /// a parameter binder which can model bind multiple parameters from the body.
    /// </summary>
    [CustomControllerConfig]
    public class SampleController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }

        [HttpPost]
        public HttpResponseMessage PostMultipleParametersFromBody(string firstname, string lastname)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.Accepted,
                String.Format("PostMultipleParametersFromBody FirstName = '{0}', LastName = '{1}'", firstname, lastname));
        }
    }
}
