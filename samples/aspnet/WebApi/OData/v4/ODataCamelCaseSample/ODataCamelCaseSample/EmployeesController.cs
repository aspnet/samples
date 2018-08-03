using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ODataCamelCaseSample
{
    public class EmployeesController : ODataController
    {
        public EmployeesController()
        {
            if (null == _employees)
            {
                InitEmployees();
            }
        }

        /// <summary>
        /// static so that the data is shared among requests.
        /// </summary>
        private static List<Employee> _employees = null;

        private static void InitEmployees()
        {
            _employees = Enumerable.Range(1, 5).Select(i =>
                        new Employee
                        {
                            ID = i,
                            FullName = "Name" + i,
                            Sex = Gender.Female,
                            Address = new Address()
                            {
                                Street = "Street" + i,
                                City = "City" + i,
                            },
                        }).ToList();
        }

        [EnableQuery(PageSize = 10, MaxExpansionDepth = 5)]
        public IHttpActionResult Get()
        {
            return Ok(_employees.AsQueryable());
        }

        public IHttpActionResult Get(int key)
        {
            return Ok(_employees.Single(e => e.ID == key));
        }

        public IHttpActionResult GetName(int key)
        {
            return Ok(_employees.Single(e => e.ID == key).FullName);
        }

        public IHttpActionResult Post(Employee employee)
        {
            employee.ID = _employees.Count + 1;
            _employees.Add(employee);

            return Created(employee);
        }
    }
}
