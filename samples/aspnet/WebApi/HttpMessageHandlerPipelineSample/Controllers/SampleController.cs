using System.Web.Http;

namespace HttpMessageHandlerPipelineSample.Controllers
{
    public class SampleController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
    }
}
