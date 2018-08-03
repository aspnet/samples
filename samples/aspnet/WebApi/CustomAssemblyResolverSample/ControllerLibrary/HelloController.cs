using System.Web.Http;

namespace ControllerLibrary
{
    public class HelloController : ApiController
    {
        public string Get()
        {
            return "Hello world from a controller library!";
        }
    }
}
