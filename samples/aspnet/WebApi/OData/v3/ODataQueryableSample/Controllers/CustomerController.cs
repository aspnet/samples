using System;
using System.Collections.Generic;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using ODataQueryableSample.Models;

namespace ODataQueryableSample.Controllers
{
    /// <summary>
    /// This sample customer controller demonstrates how to create an action which supports
    /// OData style queries using the [EnableQuery] attribute.
    /// </summary>
    public class CustomerController : ODataController
    {
        private static List<Customer> CustomerList = new List<Customer>
        {  
            new Customer { 
                Id = 11, Name = "Lowest", BirthTime = new DateTime(2001, 1, 1),
                Orders = new Order[] 
                { 
                    new Order { Id = 0 , Quantity = 10 },  
                    new Order { Id = 1 , Quantity = 50 } 
                }
            }, 
            new Customer { 
                Id = 33, Name = "Highest", BirthTime = new DateTime(2002, 2, 2),
                Orders = new Order[] 
                { 
                    new Order { Id = 2 , Quantity = 10 }, 
                    new Order { Id = 3 , Quantity = 5 } 
                }
            }, 
            new Customer { Id = 22, Name = "Middle", BirthTime = new DateTime(2003, 3, 3) }, 
            new Customer { Id = 3, Name = "NewLow", BirthTime = new DateTime(2004, 4, 4) },
        };

        [EnableQuery(AllowedArithmeticOperators = AllowedArithmeticOperators.Add)]
        public IEnumerable<Customer> Get()
        {
            return CustomerList;
        }
    }
}
