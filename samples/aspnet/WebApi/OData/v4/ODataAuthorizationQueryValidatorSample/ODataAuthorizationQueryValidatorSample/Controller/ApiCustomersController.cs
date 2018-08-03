using System.Linq;
using System.Web.Http;
using ODataAuthorizationQueryValidatorSample.Extensions;
using ODataAuthorizationQueryValidatorSample.Model;

namespace ODataAuthorizationQueryValidatorSample.Controller
{
    public class ApiCustomersController : ApiController
    {
        [AuthorizationEnableQuery]
        public IHttpActionResult Get()
        {
            IQueryable<Customer> customers = Enumerable.Range(1, 10).Select(i => new Customer
            {
                Id = i,
                Name = "Name " + i,
                Orders = Enumerable.Range(1, i).Select(j => new Order { Id = j }).ToList()
            }).AsQueryable();
            return Ok(customers);
        }
    }
}
