using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CustomModelBinderSample.Models;

namespace CustomModelBinderSample.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private static List<Product> allProducts = null;
        
        static ProductsController()
        {
            // products
            allProducts = new List<Product>();
            allProducts.Add(new Product()
                {
                    Id = 1,
                    Name = "Contoso 2014 Soccer World Cup replica ball",
                    Manufacturer = "Contoso",
                    Category = ProductCategoryType.Sports
                });
            allProducts.Add(new Product()
            {
                Id = 2,
                Name = "CLR and C#",
                Manufacturer = "Contoso Press",
                Category = ProductCategoryType.Books
            });
            allProducts.Add(new Product()
            {
                Id = 3,
                Name = "Contoso Mens Tee Shirt",
                Manufacturer = "Contoso",
                Category = ProductCategoryType.Clothing
            });
        }

        [HttpGet]
        [Route("search")]
        public IHttpActionResult Search([FromUri] SearchCriteria criteria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<Product> filteredProducts = allProducts;

            // filter by category
            if (criteria.Categories.Count > 0)
            {
                filteredProducts = allProducts.Where(product => criteria.Categories.Contains(product.Category));
            }

            // filter by search term
            if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
            {
                filteredProducts = filteredProducts.Where(product => product.Name.IndexOf(criteria.SearchTerm, 
                                                                        StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return Ok(filteredProducts);
        }
    }
}
