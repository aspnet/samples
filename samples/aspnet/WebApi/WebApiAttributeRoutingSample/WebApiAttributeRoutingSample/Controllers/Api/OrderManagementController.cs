using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiAttributeRoutingSample.Models;

namespace WebApiAttributeRoutingSample.Controllers.Api
{
    /// <summary>
    /// This controller demonstrates RPC style of accessing actions by decorating the controller-level
    /// route with the '{action}' route variable.
    /// </summary>
    [Route("api/ordermanagement/{id}/{action}")]
    public class OrderManagementController : ApiController
    {
        private ShoppingContext db = new ShoppingContext();

        // PUT api/ordermanagement/10/approveorder
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> ApproveOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.IsApproved = true;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT api/ordermanagement/10/rejectorder
        [HttpPut]
        [ActionName("RejectOrder")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> DisapproveOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.IsApproved = false;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
