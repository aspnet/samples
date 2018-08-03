using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ODataSingletonSample
{
    internal class SingletonEdmModel
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Employee> employeesConfiguration = builder.EntitySet<Employee>("Employees");
            EntityTypeConfiguration<Employee> employeeConfiguration = employeesConfiguration.EntityType;
            employeeConfiguration.Collection.Action("ResetDataSource");

            SingletonConfiguration<Company> companyConfiguration = builder.Singleton<Company>("Umbrella");
            companyConfiguration.EntityType.Action("ResetDataSource");
            companyConfiguration.EntityType.Function("GetEmployeesCount").Returns<int>();

            builder.Namespace = typeof(Company).Namespace;

            return builder.GetEdmModel();
        }
    }
}
