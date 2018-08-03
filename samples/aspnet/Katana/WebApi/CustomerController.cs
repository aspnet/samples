using System;
using System.Web.Http;

namespace WebApi
{
    public class CustomerController : ApiController
    {
        public CustomerController()
        {
        }

        // Gets
        [HttpGet]
        public Customer Get(string customerId)
        {
            return new Customer()
            {
                ID = Int32.Parse(customerId),
                LastName = "Smith",
                FirstName = "Mary",
                HouseNumber = "333",
                Street = "Main Street NE",
                City = "Redmond",
                State = "WA",
                ZipCode = "98053"
            };
        }
    }
}