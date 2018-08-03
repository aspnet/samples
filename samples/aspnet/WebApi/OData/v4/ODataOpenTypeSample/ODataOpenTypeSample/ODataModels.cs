using System.Web.OData.Builder;
using Microsoft.OData.Edm;

namespace ODataOpenTypeSample
{
    public class ODataModels
    {
        public static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Account> accounts = builder.EntitySet<Account>("Accounts");

            builder.Namespace = typeof(Account).Namespace;

            var edmModel = builder.GetEdmModel();
            return edmModel;
        }
    }
}
