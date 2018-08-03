using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ODataEnumTypeSample
{
    public class EmployeesController : ODataController
    {
        /// <summary>
        /// static so that the data is shared among requests.
        /// </summary>
        private static IList<Employee> _employees = null;

        public EmployeesController()
        {
            if (_employees == null)
            {
                InitEmployees();
            }
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

        public IHttpActionResult GetAccessLevelFromEmployee(int key)
        {
            return Ok(_employees.Single(e => e.ID == key).AccessLevel);
        }

        public IHttpActionResult GetNameFromEmployee(int key)
        {
            return Ok(_employees.Single(e => e.ID == key).Name);
        }

        public IHttpActionResult Post(Employee employee)
        {
            employee.ID = _employees.Count + 1;
            _employees.Add(employee);

            return Created(employee);
        }

        [HttpPost]
        [ODataRoute("Employees({key})/ODataEnumTypeSample.AddAccessLevel")]
        public IHttpActionResult AddAccessLevel(int key, ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            String accessLevelOfString = (String)parameters["AccessLevel"];
            AccessLevel accessLevelOfAccesslevel = (AccessLevel)Enum.Parse(typeof(AccessLevel), accessLevelOfString);

            Employee employee = _employees.Single(e => e.ID == key);
            if (null == employee)
            {
                return BadRequest();
            }

            employee.AccessLevel |= accessLevelOfAccesslevel;

            return Ok(employee.AccessLevel);
        }

        [HttpGet]
        [ODataRoute("HasAccessLevel(ID={id},AccessLevel={accessLevel})")]
        public IHttpActionResult HasAccessLevel([FromODataUri] int id, [FromODataUri] AccessLevel accessLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Employee employee = _employees.Single(e => e.ID == id);
            var result = employee.AccessLevel.HasFlag(accessLevel);
            return Ok(result);
        }

        private void InitEmployees()
        {
            _employees = new List<Employee>
            {
                new Employee()
                {
                    ID = 1,
                    Name = "Lisa",
                    Gender = Gender.Female,
                    AccessLevel = AccessLevel.Execute,
                },
                new Employee()
                {
                    ID = 2,
                    Name = "Bob",
                    Gender = Gender.Male,
                    AccessLevel = AccessLevel.Read,
                },
                new Employee(){
                    ID = 3,
                    Name = "Alice",
                    Gender = Gender.Female,
                    AccessLevel = AccessLevel.Read | AccessLevel.Write,
                },
            };
        }
    }
}
