using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using ODataQueryableSample.Models;
using System.Web.OData.Routing;

namespace ODataQueryableSample.Controllers
{
    /// <summary>
    /// This sample customer controller demonstrates how to create an action which supports
    /// OData style queries using the [EnableQuery] attribute.
    /// </summary>
    public class CustomersController : ODataController
    {
        private static List<Customer> CustomerList = new List<Customer>
        {  
            new Customer { 
                Id = 11, Name = "Lowest", Gender = Gender.Female, BirthTime = new DateTime(2001, 1, 1),
                Orders = new List<Order>
                { 
                    new Order { Id = 0 , Quantity = 10, Origin = new Origin() { City = "East", PostCode = 1024 }},
                    new Order { Id = 1 , Quantity = 50, Origin = new Origin() { City = "West", PostCode = 4096 }}
                }
            }, 
            new Customer { 
                Id = 33, Name = "Highest", Gender = Gender.Male, BirthTime = new DateTime(2002, 2, 2),
                Orders = new List<Order>
                { 
                    new Order { Id = 2 , Quantity = 10, Origin = new Origin() {City = "North", PostCode = 2048 }},
                    new Order { Id = 3 , Quantity = 5, Origin = new Origin() {City = "South", PostCode = 8192 }}
                }
            }, 
            new Customer { Id = 22, Name = "Middle", Gender = Gender.Female, BirthTime = new DateTime(2003, 3, 3) },
            new Customer { Id = 3, Name = "NewLow", Gender = Gender.Male, BirthTime = new DateTime(2004, 4, 4) },
        };

        [EnableQuery(AllowedArithmeticOperators = AllowedArithmeticOperators.Add)]
        public IEnumerable<Customer> Get()
        {
            return CustomerList;
        }

        [HttpDelete]
        public IHttpActionResult DeleteRef(int key, int relatedKey, string navigationProperty)
        {
            var customer = CustomerList.Single(c => c.Id == key);
            var order = customer.Orders.Single(o => o.Id == relatedKey);

            if (navigationProperty != "Orders")
                return BadRequest();
            customer.Orders.Remove(order);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
