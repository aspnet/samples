using System.Collections.Generic;

namespace JSComponentGeneration.Build.Common
{
    internal class RazorComponentDescriptor
    {
        public string Name { get; }

        public IReadOnlyList<BoundAttributeDescriptor> Parameters { get; }

        public RazorComponentDescriptor(TagHelperDescriptor tagHelper)
        {
            Name = GetComponentTypeName(tagHelper.Name);
            Parameters = tagHelper.BoundAttributes;
        }

        private static string GetComponentTypeName(string fullName)
            => fullName.Substring(fullName.LastIndexOf('.') + 1);
    }
}
