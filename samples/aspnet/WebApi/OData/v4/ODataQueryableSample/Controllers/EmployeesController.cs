using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Query;
using Microsoft.OData.Core;
using ODataQueryableSample.Models;

namespace ODataQueryableSample.Controllers
{
    public class EmployeesController : ODataController
    {
        public EmployeesController() 
        {
            if (null == _employees)
            {
                Initialize();
            }
        }

        private static List<Employee> _employees = null;

        private static void Initialize()
        {
            _employees = Enumerable.Range(0, 5).Select(i =>
                       new Employee
                       {
                           ID = i,
                           Name = "Name" + i
                       }).ToList();

            _employees.AddRange(Enumerable.Range(5, 2).Select(i => 
                    new Manager
                    {
                        ID = i,
                        Name = "Name" + i
                    }));

            // Manager:
            // 0,1,2->5
            // 3,4,5->6
            _employees.Single(e => e.ID == 0).Manager = (Manager)_employees.Single(e => e.ID == 5);
            _employees.Single(e => e.ID == 1).Manager = (Manager)_employees.Single(e => e.ID == 5);
            _employees.Single(e => e.ID == 2).Manager = (Manager)_employees.Single(e => e.ID == 5);
            _employees.Single(e => e.ID == 3).Manager = (Manager)_employees.Single(e => e.ID == 6);
            _employees.Single(e => e.ID == 4).Manager = (Manager)_employees.Single(e => e.ID == 6);
            _employees.Single(e => e.ID == 5).Manager = (Manager)_employees.Single(e => e.ID == 6);

            ((Manager)_employees.Single(e => e.ID == 5)).DirectReports = new List<Employee> 
            {
                _employees.Single(e => e.ID == 0),
                _employees.Single(e => e.ID == 1),
                _employees.Single(e => e.ID == 2)
            };
            ((Manager)_employees.Single(e => e.ID == 6)).DirectReports = new List<Employee> 
            {
                _employees.Single(e => e.ID == 3),
                _employees.Single(e => e.ID == 4),
                _employees.Single(e => e.ID == 5)
            };

            // Friend:
            // 0->1->2->3->0
            _employees.Single(e => e.ID == 0).Friend = _employees.Single(e => e.ID == 1);
            _employees.Single(e => e.ID == 1).Friend = _employees.Single(e => e.ID == 2);
            _employees.Single(e => e.ID == 2).Friend = _employees.Single(e => e.ID == 3);
            _employees.Single(e => e.ID == 3).Friend = _employees.Single(e => e.ID == 0);
        }

        [EnableQuery(MaxExpansionDepth = 5)]
        public IEnumerable<Employee> Get()
        {
            return _employees;
        }

        // Pass ODataQueryOptions as parameter, and call validation manually
        public IHttpActionResult GetFromManager(ODataQueryOptions<Manager> queryOptions) 
        {
            if (queryOptions.SelectExpand != null) 
            {
                queryOptions.SelectExpand.LevelsMaxLiteralExpansionDepth = 5;                
            }

            var validationSettings = new ODataValidationSettings { MaxExpansionDepth = 5 };

            try
            {
                queryOptions.Validate(validationSettings);
            }
            catch (ODataException e)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                responseMessage.Content = new StringContent(
                    string.Format("The query specified in the URI is not valid. {0}", e.Message));
                return ResponseMessage(responseMessage);
            }

            var querySettings = new ODataQuerySettings();
            var result = queryOptions.ApplyTo(_employees.OfType<Manager>().AsQueryable(), querySettings).AsQueryable();
            return Ok(result, result.GetType());
        }

        private IHttpActionResult Ok(object content, Type type)
        {
            var resultType = typeof(OkNegotiatedContentResult<>).MakeGenericType(type);
            return Activator.CreateInstance(resultType, content, this) as IHttpActionResult;
        }
    }
}
