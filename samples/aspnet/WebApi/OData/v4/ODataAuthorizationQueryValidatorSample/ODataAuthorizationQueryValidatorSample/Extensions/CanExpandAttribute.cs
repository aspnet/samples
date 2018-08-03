using System;
using Microsoft.OData.Edm;

namespace ODataAuthorizationQueryValidatorSample.Extensions
{
    /// <summary>
    /// Defines which roles can use $expand to expand navigation properties whose target entity set or entity type is
    /// indicated in the attribute.
    /// For example, if this attribute receives a type as a parameter, any navigation property whose target end is of
    /// that type, will be able to be expanded only by the roles indicated.
    /// If this attribute receives a list of navigation sources, it will mean that the roles indicated in the attribute
    /// can expand navigation properties whose target navigation source is indicated on the navigation sources list.
    /// 
    /// This attribute will apply to all the subclasses on the hierarchy. However, it's not a good design to 
    /// restrict access to data in this fashion. It may be better to seal classes where you apply this attribute or
    /// to use a different approach.
    /// For example it is possible to leak data if an instance of an entity on a 
    /// property of the base type happens to be of the derived entity type or if the subclass has properties that
    /// should be kept private.
    /// An example alternate approach is to apply restrictions on each individual property of all entity types and
    /// their subclasses and validate that the user has access to all the selected individual properties in the 
    /// $select and $expand clause.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CanExpandAttribute : Attribute
    {
        private string[] _parsedRoles;

        /// <summary>
        /// Defines that only the roles indicated in the attribute can expand properties that contain this attribute.
        /// </summary>
        /// <param name="roles">A comma-separated list of roles.</param>
        public CanExpandAttribute(string roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException("roles");
            }

            _parsedRoles = ParseValue(roles);
        }

        /// <summary>
        /// Applies the annotation to the elements in  model.
        /// </summary>
        /// <param name="model">The model to annotate.</param>
        public void SetRoles(IEdmModel model, Type clrType)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            // Apply the annotation to the IEdmEntityType represented by the CLR type. This will make any expanded 
            // navigation property whose target navigation has this entity type, only expandable by the roles in this 
            // attribute.
            model.SetAuthorizedRolesOnType(clrType.FullName, _parsedRoles);
        }

        private string[] ParseValue(string value)
        {
            return value.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
