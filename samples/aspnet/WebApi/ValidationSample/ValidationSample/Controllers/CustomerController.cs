using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ValidationSample.Models;

namespace ValidationSample.Controllers
{
    public class CustomerController : ApiController
    {
        public HttpResponseMessage Post(Customer customer)
        {
            // Send an error response if the ModelState is invalid
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
