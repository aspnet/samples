using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.OData.Query;
using System.Web.OData.Query.Validators;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Edm;


namespace ODataAuthorizationQueryValidatorSample.Extensions
{
    /// <summary>
    /// Validates that the user has permissions to expand the properties indicated in the $expand clause.
    /// </summary>
    public abstract class AuthorizationQueryValidator : ODataQueryValidator
    {
        /// <summary>
        /// Determines if the user has permissions to access an <see cref="IEdmNavigationProperty"/> whose target 
        /// entity type is the one indicated in <paramref name="edmEntityType"/>.
        /// </summary>
        /// <param name="edmEntityType">The target type of the <see cref="IEdmNavigationProperty"/></param>
        /// <returns>True if the user has permissions to access, false otherwise.</returns>
        public abstract bool CanAccess(IEdmEntityType edmEntityType);

        /// <inheritdoc />
        public override void Validate(ODataQueryOptions options, ODataValidationSettings validationSettings)
        {
            base.Validate(options, validationSettings);

            if (options.SelectExpand == null)
            {
                // Nothing to validate.
                return;
            }

            // We only validate the $expand operator, so a different mechanism is required to restrict access to a
            // navigation source directly. For example, we restrict $expand=Orders, but we don't restrict accessing
            // orders through /Orders. The recommendation for restricting access to a navigation source in the URL path
            // is to use an IAuthorizationFilter.
            SelectExpandClause selectExpandClause = options.SelectExpand.SelectExpandClause;
            IList<ExpandedNavigationSelectItem> expandedItems = GetExpandedProperties(selectExpandClause);
            foreach (ExpandedNavigationSelectItem item in expandedItems)
            {
                ValidateExpandedItem(item);
            }
        }

        private void ValidateExpandedItem(ExpandedNavigationSelectItem item)
        {
            // If the navigation source doesn't exist, means that we are in query composition mode, so we just examine
            // the $expand path (The list of navigation properties) and check the annotations on their IEdmTypes.
            if (item.NavigationSource != null && CanAccess(item.NavigationSource.EntityType()))
            {
                foreach (var element in GetExpandedProperties(item.SelectAndExpand))
                {
                    ValidateExpandedItem(element);
                }
            }
            else if (item.NavigationSource == null)
            {
                IEnumerable<NavigationPropertySegment> path = item.PathToNavigationProperty.OfType<NavigationPropertySegment>();
                ValidateExpandedPath(path);
            }
            else
            {
                var segment = item.PathToNavigationProperty.OfType<NavigationPropertySegment>().First();
                string message = string.Format("Not authorized to expand the navigation property '{0}'",
                    segment.NavigationProperty.Name);

                throw new ODataException(message);
            }
        }

        private void ValidateExpandedPath(IEnumerable<NavigationPropertySegment> path)
        {
            foreach (var segment in path)
            {
                if (!CanAccess(segment.NavigationProperty.ToEntityType()))
                {
                    string message = string.Format("Not authorized to expand the navigation property '{0}'",
                        segment.NavigationProperty.Name);

                    throw new ODataException(message);
                }
            }
        }

        private static IList<ExpandedNavigationSelectItem> GetExpandedProperties(SelectExpandClause clause)
        {
            Contract.Assert(clause != null);
            return clause.SelectedItems.OfType<ExpandedNavigationSelectItem>().ToList();
        }
    }
}
