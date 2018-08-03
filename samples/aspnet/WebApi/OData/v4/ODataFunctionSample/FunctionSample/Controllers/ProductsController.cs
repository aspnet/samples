using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using FunctionSample.Models;

namespace FunctionSample.Controllers
{
    public class ProductsController : ODataController
    {
        private static ConcurrentDictionary<int, Product> _data;

        static ProductsController()
        {
            _data = new ConcurrentDictionary<int, Product>();
            var rand = new Random();

            Enumerable.Range(0, 100)
                .Select(i => new Product
                {
                    Id = i,
                    Name = "Product " + i,
                    Price = rand.NextDouble() * 1000
                })
                .ToList()
                .ForEach(p => _data.TryAdd(p.Id, p));
        }

        public ProductsController()
        {
        }

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return _data.Values.AsQueryable();
        }

        public IHttpActionResult GetProduct(int key)
        {
            Product retval;
            if (_data.TryGetValue(key, out retval))
            {
                return Ok(retval);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult MostExpensive()
        {
            var retval = _data.Max(pair => pair.Value.Price);

            return Ok(retval);
        }

        // Returns the top ten most expensive products
        [HttpGet]
        public IHttpActionResult Top10()
        {
            var retval = _data.Values.OrderByDescending(p => p.Price).Take(10).ToList();

            return Ok(retval);
        }

        [HttpGet]
        public IHttpActionResult GetPriceRank(int key)
        {
            Product p;
            if (_data.TryGetValue(key, out p))
            {
                // NOTE: Use where clause to get the rank of the price may not
                // offer the good time complexity. The following code is intended
                // for demostration only.
                return Ok(_data.Values.Where(one => one.Price > p.Price).Count());
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult CalculateGeneralSalesTax(int key, string state)
        {
            double taxRate = GetRate(state);

            Product product;
            if (_data.TryGetValue(key, out product))
            {
                double tax = product.Price * taxRate / 100;
                return Ok(tax);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ODataRoute("GetSalesTaxRate(state={state})")]
        public IHttpActionResult GetSalesTaxRate([FromODataUri] string state)
        {
            return Ok(GetRate(state));
        }

        private static double GetRate(string state)
        {
            double taxRate = 0;
            switch (state)
            {
                case "AZ": taxRate = 5.6; break;
                case "CA": taxRate = 7.5; break;
                case "CT": taxRate = 6.35; break;
                case "GA": taxRate = 4; break;
                case "IN": taxRate = 7; break;
                case "KS": taxRate = 6.15; break;
                case "KY": taxRate = 6; break;
                case "MA": taxRate = 6.25; break;
                case "NV": taxRate = 6.85; break;
                case "NJ": taxRate = 7; break;
                case "NY": taxRate = 4; break;
                case "NC": taxRate = 4.75; break;
                case "ND": taxRate = 5; break;
                case "PA": taxRate = 6; break;
                case "TN": taxRate = 7; break;
                case "TX": taxRate = 6.25; break;
                case "VA": taxRate = 4.3; break;
                case "WA": taxRate = 6.5; break;
                case "WV": taxRate = 6; break;
                case "WI": taxRate = 5; break;

                default:
                    taxRate = 0;
                    break;
            }

            return taxRate;
        }
    }
}
