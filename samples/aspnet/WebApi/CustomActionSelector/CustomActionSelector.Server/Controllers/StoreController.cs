// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CustomActionSelector.Server
{
    [BetaTestingBehavior]
    public class StoreController : ApiController
    {
        private static readonly Product[] _products = new Product[]
        {
            new Product()
            {
                Id = 1,
                Name = "Contoso Widget",
                Price = 19.95m,
                Description = "Contoso has for many years been the most trusted name in Widgets. Our Widgets are fully guaranteed and will last for years."
            },
            new Product()
            {
                Id = 2,
                Name = "Contoso Widget Cleaning Spray",
                Price = 5.74m,
                Description = "Keep your Widgets looking new with this handy cleaning spray. Just $5.75 for 12oz of our cleaning solution."
            }
        };

        private static readonly Product[] _betaProducts = new Product[]
        {
            new Product()
            {
                Id = 1,
                Name = "Contoso Widget",
                Price = 19.95m,
                Description = "Contoso Widgets RULE - ours are the best in the business plain and simple. Contoso! NOTHING BUT THE BEST."
            },
            new Product()
            {
                Id = 2,
                Name = "Contoso Widget Cleaning Spray",
                Price = 5.74m,
                Description = "This cleaning spray will actually whiten your teeth and build muscle while you use it. Contoso! NOTHING BUT THE BEST."
            }
        };

        [HasBetaTestingAction("GetAllBetaProducts")]
        public IHttpActionResult GetAllProducts()
        {
            return Ok(_products);
        }

        private IHttpActionResult GetAllBetaProducts()
        {
            return Ok(_betaProducts);
        }

        [HasBetaTestingAction("GetBetaProduct")]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _products.Where(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        private IHttpActionResult GetBetaProduct(int id)
        {
            var product = _betaProducts.Where(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }
    }
}