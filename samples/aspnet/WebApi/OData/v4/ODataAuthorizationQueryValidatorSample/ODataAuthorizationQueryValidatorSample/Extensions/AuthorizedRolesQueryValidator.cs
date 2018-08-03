using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Principal;
using Microsoft.OData.Edm;

namespace ODataAuthorizationQueryValidatorSample.Extensions
{
    /// <summary>
    /// Validates that the <see cref="IPrincipal"/> associated with the request is allowed to access all the segments
    /// in an $expand path by ensuring that is a member of the roles each segment is restricted to.
    /// </summary>
    public class AuthorizedRolesQueryValidator : AuthorizationQueryValidator
    {
        private IEdmModel _model;
        private IPrincipal _principal;

        /// <summary>
        /// Creates a new instance of the <see cref="AuthorizedRolesQueryValidator"/> class.
        /// </summary>
        /// <param name="model">The <see cref="IEdmModel"/> associated with the request.</param>
        /// <param name="principal">The <see cref="IPrincipal"/> associated with the request</param>
        public AuthorizedRolesQueryValidator(IEdmModel model, IPrincipal principal)
        {
            _model = model;
            _principal = principal;
        }

        public override bool CanAccess(IEdmEntityType edmEntityType)
        {
            AuthorizedRolesAnnotation authorizedRoles = GetAuthorizedRolesAnnotation(edmEntityType);
            return IsAuthorized(authorizedRoles);
        }

        private bool IsAuthorized(AuthorizedRolesAnnotation authorizedRoles)
        {
            return authorizedRoles == null ||
                   authorizedRoles.AllowedRoles.Any(role => role == "*" ||
                       _principal.IsInRole(role));
        }

        private AuthorizedRolesAnnotation GetAuthorizedRolesAnnotation(IEdmType entityType)
        {
            IEdmCollectionType collection = entityType as IEdmCollectionType;
            IEdmType type = collection != null ?
                collection.ElementType.Definition :
                entityType as IEdmEntityType;

            Contract.Assert(type != null);
            return _model.GetAnnotationValue<AuthorizedRolesAnnotation>(type);
        }
    }
}
