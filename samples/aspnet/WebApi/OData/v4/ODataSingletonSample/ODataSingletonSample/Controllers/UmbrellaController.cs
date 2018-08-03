using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ODataSingletonSample
{
    /// <summary>
    /// Present a singleton named "Umbrella"
    /// Use convention routing
    /// </summary>
    public class UmbrellaController : ODataController
    {
        public static Company Umbrella;

        static UmbrellaController()
        {
            InitData();
        }

        private static void InitData()
        {
            Umbrella = new Company()
            {
                ID = 1,
                Name = "Umbrella",
                Revenue = 1000,
                Category = CompanyCategory.Communication,
                Employees = new List<Employee>()
            };
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(Umbrella);
        }

        public IHttpActionResult GetRevenueFromCompany()
        {
            return Ok(Umbrella.Revenue);
        }

        public IHttpActionResult GetName()
        {
            return Ok(Umbrella.Name);
        }

        public IHttpActionResult GetEmployeesFromCompany()
        {
            return Ok(Umbrella.Employees);
        }

        public IHttpActionResult Put(Company newCompany)
        {
            Umbrella = newCompany;
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult Patch(Delta<Company> item)
        {
            item.Patch(Umbrella);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [AcceptVerbs("POST")]
        public IHttpActionResult CreateRef(string navigationProperty, [FromBody] Uri link)
        {
            int relatedKey = HelperFunction.GetKeyValue<int>(link);
            Employee employee = EmployeesController.Employees.First(x => x.ID == relatedKey);

            if (navigationProperty != "Employees" || employee == null)
            {
                return BadRequest();
            }

            if (Umbrella.Employees == null)
            {
                Umbrella.Employees = new List<Employee>() {employee};
            }
            else
            {
                Umbrella.Employees.Add(employee);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [AcceptVerbs("DELETE")]
        public IHttpActionResult DeleteRef(string relatedKey, string navigationProperty)
        {
            int key = int.Parse(relatedKey);
            Employee employee = Umbrella.Employees.First(x => x.ID == key);

            if (navigationProperty != "Employees")
            {
                return BadRequest();
            }

            Umbrella.Employees.Remove(employee);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public IHttpActionResult PostToEmployees([FromBody] Employee employee)
        {
            EmployeesController.Employees.Add(employee);
            if (Umbrella.Employees == null)
            {
                Umbrella.Employees = new List<Employee>() { employee };
            }
            else
            {
                Umbrella.Employees.Add(employee);
            }

            return Created(employee);
        }

        [HttpPost]
        [ODataRoute("Umbrella/ODataSingletonSample.ResetDataSource")]
        public IHttpActionResult ResetDataSourceOnCompany()
        {
            InitData();
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult GetEmployeesCount()
        {
            return Ok(Umbrella.Employees.Count);
        }
    }
}
