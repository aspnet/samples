using System.Web.Http;

namespace WebApiAttributeRoutingSample.Controllers.Api
{
    // These controllers demonstrate the use of attribute routing
    // for one way of achieiving versioning of your controllers
    
    [Route("api/v1/countries")]
    public class CountriesV1Controller : ApiController
    {
        // GET api/v1/countries
        public string Get()
        {
            return "Response from CountriesV1";
        }
    }

    [Route("api/v2/countries")]
    public class CountriesV2Controller : ApiController
    {
        // GET api/v2/countries
        public string Get()
        {
            return "Response from CountriesV2";
        }
    } 
}