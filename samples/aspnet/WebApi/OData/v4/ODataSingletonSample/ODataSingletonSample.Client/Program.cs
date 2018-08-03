using System;
using System.Linq;
using System.Threading;
using Microsoft.OData.Client;
using ODataSingletonSample.Client;

namespace SingletonClientSample.Client
{
    public class Program
    {
        private static readonly string _baseAddress = "http://localhost:50268/odata";  //Set to http://localhost.fiddler:50268/odata if need to capture request in fiddler

        /// <summary>
        /// Singleton Sample Code
        /// Umbrella is a singleton, Employees is an entityset
        /// Umbrella has a navigation property "Employees", which is binded to the Employees entityset
        /// Employees has a navigation property "Company", which is binded to Umbrella
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Sleep for some time to give IIS Express time to start the service
            Thread.Sleep(2000);

            Container clientContext = new Container(new Uri(_baseAddress));
            clientContext.Format.UseJson();
            clientContext.MergeOption = MergeOption.OverwriteChanges;

            // Query Singleton
            var company = clientContext.Umbrella.Single();

            // Call bound action to reset singleton
            company.ResetDataSource();

            // Call bound action to reset entityset
            clientContext.Execute(new Uri(clientContext.BaseUri + "/Employees/ODataSingletonSample.ResetDataSource"), "POST");
            company = clientContext.Umbrella.Single();
            Console.WriteLine("Company name is: " + company.Name);
            Console.WriteLine("Company revenue is: " + company.Revenue);

            // Update singleton by PATCH
            company.Name = "Umbrella-NewName";
            clientContext.UpdateObject(company);
            clientContext.SaveChanges();

            // Query singleton property
            var name = clientContext.Execute<string>(new Uri("Umbrella/Name", UriKind.Relative)).Single();
            Console.WriteLine("After update Company name by PATCH, Company name is: " + name);
            
            // Update singleton by PUT
            company.Revenue = 1200;
            clientContext.UpdateObject(company);
            clientContext.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);

            // Query singleton property
            var revenue = clientContext.Execute<Int64>(new Uri("Umbrella/Revenue", UriKind.Relative)).Single();
            Console.WriteLine("After update Company revenue by PUT, Company revenue is: " + revenue);

            // Add navigation link by creating a new entity
            Employee newEmployee1 = new Employee() { ID = 1111, Name = "NewHired1" };
            clientContext.AddRelatedObject(company, "Employees", newEmployee1);
            clientContext.SaveChanges();

            // Add navigation link based on existing entity
            Employee newEmployee2 = new Employee() { ID = 2222, Name = "NewHired2" };

            clientContext.AddToEmployees(newEmployee2);
            clientContext.SaveChanges();
            clientContext.AddLink(company, "Employees", newEmployee2);
            clientContext.SaveChanges();

            // Call unbound function
            var employeesCount = company.GetEmployeesCount();
            Console.WriteLine("After adding employees to Company, Company employees count is: " + employeesCount);

            // Load property
            Console.WriteLine("Company Employees names are: ");
            clientContext.LoadProperty(company, "Employees");
            var employees = company.Employees;
            foreach (var emp in employees)
            {
                Console.WriteLine(emp.Name);
            }

            // Query Option - $select
            Console.WriteLine("Execute: GET http://localhost:50268/odata/Umbrella?$select=Name,Revenue");
            clientContext.Umbrella.Select(c => new Company {Name = c.Name, Revenue = c.Revenue}).SingleOrDefault();

            // Query Option - $expand
            Console.WriteLine("Execute: GET http://localhost:50268/odata/Umbrella?$expand=Employees");
            clientContext.Umbrella.Expand(c => c.Employees).SingleOrDefault();

            // Delete navigation link
            Console.WriteLine("Delete navigation link: DELETE http://localhost:50268/odata/Umbrella/Employees/$ref?$id=http://localhost:50268/odata/Employees(1111) ");
            Employee employeeToDelete = clientContext.Employees.Where(e => e.ID == newEmployee1.ID).Single();
            clientContext.DeleteLink(company, "Employees", employeeToDelete);
            clientContext.SaveChanges();

            // Call unbound function
            employeesCount = company.GetEmployeesCount();
            Console.WriteLine("After deleting employees of Company, Company employees count is: " + employeesCount);

            // singleton as navigation target
            Console.WriteLine("Associate Company to Employees(1)");
            var employee = clientContext.Employees.Where(e => e.ID == 1).Single();
            clientContext.SetLink(employee, "Company", company);
            clientContext.SaveChanges();

            clientContext.LoadProperty(employee, "Company");
            Console.WriteLine("Employees(1)'s Company is: " + employee.Company.Name);
           
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
    }
}
