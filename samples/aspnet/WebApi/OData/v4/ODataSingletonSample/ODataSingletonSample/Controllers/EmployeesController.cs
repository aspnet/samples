using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;

namespace ODataSingletonSample
{
    public class EmployeesController : ODataController
    {
        public static List<Employee> Employees;

        static EmployeesController()
        {
            InitData();
        }

        private static void InitData()
        {
            Employees = Enumerable.Range(0, 10).Select(i =>
                   new Employee()
                   {
                       ID = i,
                       Name = string.Format("Name {0}", i)
                   }).ToList();
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(Employees.AsQueryable());
        }

        public IHttpActionResult Get(int key)
        {
            return Ok(Employees.Where(e => e.ID == key));
        }

        [EnableQuery]
        public IHttpActionResult GetEmployees()
        {
            return Ok(Employees.AsQueryable());
        }

        public IHttpActionResult GetCompanyFromEmployee([FromODataUri] int key)
        {
            var company = Employees.First(e => e.ID == key).Company;
            if (company == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
            return Ok(company);
        }

        public IHttpActionResult POST([FromBody] Employee employee)
        {
            Employees.Add(employee);
            return Created(employee);
        }

        [AcceptVerbs("PUT")]
        public IHttpActionResult CreateRef([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
        {
            if (navigationProperty != "Company")
            {
                return BadRequest();
            }

            Employees.First(e => e.ID == key).Company = UmbrellaController.Umbrella;
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult DeleteRef([FromODataUri] int key, string navigationProperty)
        {
            if (navigationProperty != "Company")
            {
                return BadRequest();
            }

            Employees.First(e => e.ID == key).Company = null;
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult PutToCompany(int key, Company company)
        {
            var navigateCompany = Employees.First(e => e.ID == key).Company;
            Employees.First(e => e.ID == key).Company = company;
            if (navigateCompany.Name == "Umbrella")
            {
                UmbrellaController.Umbrella = navigateCompany;
            }
            else
            {
                return BadRequest();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        public IHttpActionResult PatchToCompany(int key, Delta<Company> company)
        {
            var navigateCompany = Employees.First(e => e.ID == key).Company;
            company.Patch(Employees.First(e => e.ID == key).Company);
            if (navigateCompany.Name == "Umbrella")
            {
                company.Patch(UmbrellaController.Umbrella);
            }
            else
            {
                return BadRequest();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public IHttpActionResult ResetDataSourceOnCollectionOfEmployee()
        {
            InitData();
            return Ok();
        }
    }
}
