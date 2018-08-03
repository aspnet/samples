using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ProductODataService.Models;

namespace ProductODataService.Controllers
{
    public class ProductsController : ODataController
    {
        private static List<Product> _products = null;

        static ProductsController()
        {
            _products = Enumerable.Range(1, 5).Select(i =>
            new Product
            {
                Id = i,
                Name = "Name" + i + (i % 2 == 0 ? "/" : "\\"),

            }).ToList();
        }

        [EnableQuery(PageSize = 10, MaxExpansionDepth = 5)]
        public IHttpActionResult Get()
        {
            return Ok(_products.AsQueryable());
        }

        // ~/Employees/Namespace.GetCount()
        [HttpGet]
        public int GetCount()
        {
            return _products.Count();
        }

        // ~/Employees/Namespace.GetCount(Name='Name1')
        [HttpGet]
        public int GetCount(string Name)
        {
            return _products.Count(e => e.Name.Contains(Name));
        }
    }
}
