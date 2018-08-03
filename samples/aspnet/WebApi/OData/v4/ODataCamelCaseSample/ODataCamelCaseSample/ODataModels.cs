using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ODataCamelCaseSample
{
    public class ODataModels
    {
        public static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Employee> employees = builder.EntitySet<Employee>("Employees");
            EntityTypeConfiguration<Employee> employee = employees.EntityType;
            employee.EnumProperty<Gender>(e => e.Sex).Name = "Gender";

            var resetDataSource = builder.Action("ResetDataSource");

            builder.Namespace = typeof(Employee).Namespace;

            // All the property names in the generated Edm Model will become camel case if EnableLowerCamelCase() is called.
            builder.EnableLowerCamelCase();
            var edmModel = builder.GetEdmModel();
            return edmModel;
        }
    }
}
