using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using ExternalEdmModel.Model;

namespace ExternalEdmModel.Controllers
{
    public class CustomersController : ODataController
    {
        CustomersContext context = new CustomersContext();

        [EnableQuery(PageSize = 5)]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(context.Customers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
    }
}
