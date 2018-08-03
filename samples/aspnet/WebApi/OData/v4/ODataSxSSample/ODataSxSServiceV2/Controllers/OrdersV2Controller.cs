using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using ODataSxSServiceV2.Models;

namespace ODataSxSServiceV2.Controllers
{
    [ODataRoutePrefix("Orders")]
    public class OrdersV2Controller : ODataController
    {
        private ODataSxSServiceContext db = new ODataSxSServiceContext();

        // GET odata/Orders
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Top)]
        [ODataRoute("")]
        public IQueryable<Order> GetAllOrders()
        {
            return db.Orders;
        }

        // GET odata/Orders(5)
        [EnableQuery]
        [ODataRoute("({key})")]
        public SingleResult<Order> GetOneOrder([FromODataUri] int key)
        {
            return SingleResult.Create(db.Orders.Where(order => order.OrderId == key));
        }

        // PUT odata/Orders(5)
        public IHttpActionResult Put([FromODataUri] int key, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != order.OrderId)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(order);
        }

        // POST odata/Orders
        public IHttpActionResult Post(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return Created(order);
        }

        // PATCH odata/Orders(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Order> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order order = db.Orders.Find(key);
            if (order == null)
            {
                return NotFound();
            }

            patch.Patch(order);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(order);
        }

        // DELETE odata/Orders(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Order order = db.Orders.Find(key);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Orders(5)/Products
        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Orders.Where(m => m.OrderId == key).SelectMany(m => m.Products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int key)
        {
            return db.Orders.Count(e => e.OrderId == key) > 0;
        }
    }
}
