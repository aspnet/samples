using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace MjrChess.Trainer.BlazorExtensions
{
    /// <summary>
    /// Extension of the RouteView class for displaying layout pages accepting additional parameters
    /// from the pages they display.
    ///
    /// With the default RouteView, layout pages are instantiated with only the Body parameter. This class
    /// allows pages to optionally specify other parameters (besides Body) which are passed to the Layout page.
    /// </summary>
    /// <remarks>
    /// This allows a page to specify both a body and, for example, other components that are rendered elsewhere
    /// in the layout. In this sample project, it's used to allow pages to specify buttons to show in the navigation
    /// bar at the top of the page, separate from the rest of the body content.
    /// </remarks>
    public class ExpandedRouteView

        // It would be nice to derive this from AuthorizeRouteView, but AuthorizeRouteView is
        // sealed, so derive from RouteView, instead. Callers can use CascadingAuthenticationState
        // to get access to authentication state.
        : RouteView
    {
        public const string GetLayoutParametersName = "GetLayoutParameters";

        /// <summary>
        /// Renders the page at the specified route.
        /// </summary>
        /// <remarks>
        /// Based on https://github.com/aspnet/AspNetCore/blob/master/src/Components/Components/src/RouteView.cs and
        /// https://github.com/aspnet/AspNetCore/blob/master/src/Components/Components/src/LayoutView.cs.
        /// </remarks>
        protected override void Render(RenderTreeBuilder builder)
        {
            // Get the layout page type (either from the page or from the default)
            var pageLayoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType ?? DefaultLayout;

            // Get a list of all parameters the Layout is expecting
            var parameterAttributeType = typeof(ParameterAttribute);
            var parametersExpected = pageLayoutType.GetProperties()
                .Where(p => p.CustomAttributes.Any(a => parameterAttributeType.IsAssignableFrom(a.AttributeType)))
                .Select(p => p.Name)
                .ToList();

            // Remove the expected body parameter since it's an always present special case
            parametersExpected.Remove("Body");

            var index = 0;

            // Begin rendering the layout page
            builder.OpenComponent(index++, pageLayoutType);

            // Add the body
            builder.AddAttribute(index++, "Body", new RenderFragment(RenderPageWithParameters));

            // Add other custom attributes that the page may have specified that the Layout page knows how to handle
            AddParametersFromPage(builder, RouteData.PageType, parametersExpected, ref index);

            // Pass null for any additional expected parameters that weren't defined by the page
            foreach (var name in parametersExpected)
            {
                builder.AddAttribute(index++, name, (object)null!);
            }

            builder.CloseComponent();
        }

        private static void AddParametersFromPage(RenderTreeBuilder builder, Type pageType, List<string> layoutParameterNames, ref int index)
        {
            // We get the method for retrieving layout parameters using reflection. We can't have the pageType derive from
            // a base class or interface with a `GetLayoutParameters` method because `GetLayoutParameters` needs to be static
            // since we don't have an instance of pageType at this point and static methods can't be abstract or virtual.
            var getAttributesMethod = pageType.GetMethod(GetLayoutParametersName);
            if (getAttributesMethod != null)
            {
                if (getAttributesMethod.Invoke(null, null) is Dictionary<string, object> attributes)
                {
                    // For any attributes supplied by the page which the layout expects, add
                    // the attribute and remove it from the list of parameters still expected.
                    foreach (var attr in attributes.Where(a => layoutParameterNames.Contains(a.Key)))
                    {
                        builder.AddAttribute(index++, attr.Key, attr.Value);
                        layoutParameterNames.Remove(attr.Key);
                    }
                }
            }
        }

        private void RenderPageWithParameters(RenderTreeBuilder builder)
        {
            builder.OpenComponent(0, RouteData.PageType);

            foreach (var kvp in RouteData.RouteValues)
            {
                builder.AddAttribute(1, kvp.Key, kvp.Value);
            }

            builder.CloseComponent();
        }
    }
}
