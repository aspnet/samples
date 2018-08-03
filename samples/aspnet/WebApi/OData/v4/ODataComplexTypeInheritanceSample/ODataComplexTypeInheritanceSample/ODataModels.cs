using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ODataComplexTypeInheritanceSample
{
    public class ODataModels
    {
        public static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Window> windows = builder.EntitySet<Window>("Windows");
            EntityTypeConfiguration<Window> window = windows.EntityType;

            // Action that takes in a base complex type.
            ActionConfiguration actionConfiguration = window.Action("AddOptionalShape");
            actionConfiguration.Parameter<Shape>("shape");
            actionConfiguration.Returns<int>(); // The number of all optional shapes

            // Function that returns a base complex type
            var functionConfiguration = window.Function("GetTheLastOptionalShape");
            functionConfiguration.Returns<Shape>();

            builder.Namespace = typeof(Window).Namespace;

            return builder.GetEdmModel();
        }
    }
}
