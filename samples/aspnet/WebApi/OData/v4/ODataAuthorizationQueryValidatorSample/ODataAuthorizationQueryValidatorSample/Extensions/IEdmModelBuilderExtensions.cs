using System;
using System.Linq;
using System.Reflection;
using System.Web.OData;
using Microsoft.OData.Edm;

namespace ODataAuthorizationQueryValidatorSample.Extensions
{
    public static class IEdmModelBuilderExtensions
    {
        /// <summary>
        /// Inspect the CLR Types associated with the entity types of this <see cref="IEdmModel"/> and apply the
        /// <see cref="AuthorizedRolesAnnotation"/> if the <see cref="CanExpandAttribute"/> has been applied to the
        /// type.
        /// </summary>
        /// <param name="edmModel">The <see cref="IEdmModel"/> to annotate.</param>
        public static void AddAuthorizedRolesAnnotations(this IEdmModel edmModel)
        {
            var typeAnnotationsMapping = edmModel.SchemaElementsAcrossModels()
                .OfType<IEdmEntityType>()
                .Where(t => edmModel.GetAnnotationValue<ClrTypeAnnotation>(t) != null)
                .Select(t => edmModel.GetAnnotationValue<ClrTypeAnnotation>(t).ClrType)
                .ToDictionary(clrType => clrType,
                              clrType => clrType.GetCustomAttributes<CanExpandAttribute>(inherit: false));

            foreach (var kvp in typeAnnotationsMapping)
            {
                foreach (var attribute in kvp.Value)
                {
                    attribute.SetRoles(edmModel, kvp.Key);
                }
            }
        }

        /// <summary>
        /// Annotates the type given by the <paramref name="typeName"/> with the <see cref="AuthorizedRolesAnnotation"/>
        /// and assigns the list of <paramref name="roles"/> to that annotation.
        /// </summary>
        /// <param name="model">The <see cref="IEdmModel"/> containing the type to annotate.</param>
        /// <param name="typeName"> The fully qualified type name of the <see cref="IEdmEntityType"/> to annotate.</param>
        /// <param name="roles">The list of allowed roles for any navigation property whose tardet end is of this
        /// <paramref name="typeName"/></param>
        public static void SetAuthorizedRolesOnType(
            this IEdmModel model,
            string typeName,
            string[] roles)
        {
            IEdmEntityType type = model.FindType(typeName) as IEdmEntityType;
            if (type == null)
            {
                throw new InvalidOperationException("The authorized element must be an entity type");
            }

            model.SetAnnotationValue<AuthorizedRolesAnnotation>(type, new AuthorizedRolesAnnotation(roles));
        }
    }
}
