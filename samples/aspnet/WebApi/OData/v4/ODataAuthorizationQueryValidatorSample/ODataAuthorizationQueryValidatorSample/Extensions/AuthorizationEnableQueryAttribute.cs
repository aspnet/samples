using System;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using Microsoft.OData.Edm;
using System.Web.Http;

namespace ODataAuthorizationQueryValidatorSample.Extensions
{
    /// <summary>
    /// A version of the <see cref="EnableQueryAttribute"/> that adds an <see cref="AuthorizationQueryValidator"/>
    /// to ensure that the client has permissions to retrieve the resources indicated in the $expand path.
    /// </summary>
    public class AuthorizationEnableQueryAttribute : EnableQueryAttribute
    {
        private const string ModelKeyPrefix = "System.Web.OData.Model+";

        // Override this method to plug in our custom validator.
        public override void ValidateQuery(HttpRequestMessage request, ODataQueryOptions queryOptions)
        {
            IEdmModel model = queryOptions.Context.Model;
            IPrincipal principal = request.GetRequestContext().Principal;

            queryOptions.Validator = new AuthorizedRolesQueryValidator(model, principal);
            base.ValidateQuery(request, queryOptions);
        }

        public override IEdmModel GetModel(Type elementClrType,
            HttpRequestMessage request,
            HttpActionDescriptor actionDescriptor)
        {
            // Check to see if the request contains a model (It's an OData request or the user defined his own model).
            // if not, create one and cache it on the action descriptor for reuse later.
            IEdmModel model = request.ODataProperties().Model ??
            actionDescriptor.Properties.GetOrAdd(
                ModelKeyPrefix + elementClrType.FullName,
                _ => CreateImplicitModel(actionDescriptor.Configuration, elementClrType)) as IEdmModel;

            return model;
        }

        private static IEdmModel CreateImplicitModel(HttpConfiguration configuration, Type elementClrType)
        {
            ODataConventionModelBuilder builder =
                new ODataConventionModelBuilder(configuration, isQueryCompositionMode: true);

            // Add the type to the model as an entity and add an entity set with the same name.
            EntityTypeConfiguration entityTypeConfiguration = builder.AddEntityType(elementClrType);
            builder.AddEntitySet(elementClrType.Name, entityTypeConfiguration);

            // Build the model and add the AuthorizedRolesAnnotation.
            IEdmModel edmModel = builder.GetEdmModel();
            Contract.Assert(edmModel != null);

            edmModel.AddAuthorizedRolesAnnotations();
            return edmModel;
        }
    }
}
