
namespace ODataAuthorizationQueryValidatorSample.Extensions
{
    /// <summary>
    /// Defines which roles have access to the annotated element.
    /// </summary>
    public class AuthorizedRolesAnnotation
    {
        /// <summary>
        /// The list of roles that can access the annotated element.
        /// </summary>
        public string[] AllowedRoles { get; private set; }

        /// <summary>
        /// Creates a new <see cref="AuthorizedRolesAnnotation"/> instance.
        /// </summary>
        /// <param name="allowedRoles">The list of roles associated with this instance.</param>
        public AuthorizedRolesAnnotation(params string[] allowedRoles)
        {
            AllowedRoles = allowedRoles;
        }
    }
}
