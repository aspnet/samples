// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using System.Web.Http;
using RoutingConstraints.Server.Models;

namespace RoutingConstraints.Server.Controllers
{
    /// <summary>
    /// A Customer controller for version 1 of the api.
    /// </summary>
    [VersionedRoute("api/Customer", 1)]
    public class CustomerVersion1Controller : ApiController
    {
        private static readonly CustomerVersion1[] _customers = new CustomerVersion1[]
        {
            new CustomerVersion1()
            {
                Id = 1,
                Name = "Contoso LLC",
                Address = "1 Microsoft Way Redmond WA 98053"
            }
        };

        public IHttpActionResult Get()
        {
            return Json(_customers);
        }

        public IHttpActionResult Get(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return Json(customer);
            }
        }
    }
}
