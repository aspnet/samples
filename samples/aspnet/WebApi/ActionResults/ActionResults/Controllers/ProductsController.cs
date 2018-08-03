using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ActionResults.Controllers
{
    public class ProductsController : ApiController
    {
        private static readonly IDictionary<int, string> _defaultCatalog = new Dictionary<int, string>
        {
            { 1, "Foo" },
            { 2, "Bar" },
            { 3, "Baz" }
        };

        private readonly IDictionary<int, string> _catalog;

        public ProductsController() : this(_defaultCatalog)
        {
        }

        public ProductsController(IDictionary<int, string> products)
        {
            if (products == null)
            {
                throw new ArgumentNullException("products");
            }

            _catalog = products;
        }

        [Route("product/{id}")]
        public IHttpActionResult Get(int id)
        {
            if (!_catalog.ContainsKey(id))
            {
                return NotFound();
            }

            return Ok(_catalog[id]);
        }
    }
}