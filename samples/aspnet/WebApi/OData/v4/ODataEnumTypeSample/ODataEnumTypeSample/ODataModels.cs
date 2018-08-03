using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ODataEnumTypeSample
{
    public class ODataModels
    {
        public static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Employee> employees = builder.EntitySet<Employee>("Employees");
            EntityTypeConfiguration<Employee> employee = employees.EntityType;

            // Action that adds a skill to the skill set.
            ActionConfiguration actionConfiguration = employee.Action("AddAccessLevel");
            actionConfiguration.Parameter<string>("AccessLevel");// Now action parameters does not support Enum type, so use string instead.
            actionConfiguration.Returns<AccessLevel>();

            // Function that tells whether an employee has the given AccessLevel
            var functionConfiguration = builder.Function("HasAccessLevel");
            functionConfiguration.Parameter<int>("ID");
            functionConfiguration.Parameter<AccessLevel>("AccessLevel");
            functionConfiguration.Returns<bool>();

            builder.Namespace = typeof(Employee).Namespace;

            return builder.GetEdmModel();
        }
    }
}
