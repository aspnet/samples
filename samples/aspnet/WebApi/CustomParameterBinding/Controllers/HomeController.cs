using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Web.Http;
using CustomParameterBindingSample.Models;

namespace CustomParameterBindingSample.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage BindPrincipal(IPrincipal principal)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.Accepted,
                String.Format("BindPrincipal with Principal name {0}.", principal.Identity.Name));
        }

        [HttpGet]
        [HttpPost]
        public HttpResponseMessage BindCustomComplexTypeFromUriOrBody(TestItem item)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.Accepted, 
                String.Format("BindCustomComplexTypeFromUriOrBody returns item.Name = {0}.", item.Name));
        }

        [HttpGet]
        public HttpResponseMessage BindCustomComplexTypeFromUriWithRenamedProperty(TestItemRenameProperty item)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.Accepted,
                String.Format("BindCustomComplexTypeFromUriWithRenamedProperty item.Name = {0}.", item.Name));
        }

        [HttpPost]
        public HttpResponseMessage PostMultipleParametersFromBody(string firstname, string lastname)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.Accepted,
                String.Format("BindCustomCoPostMultipleParametersFromBodymplexType FristName = {0}, LastName = {1}.", firstname, lastname));
        }
    }
}
