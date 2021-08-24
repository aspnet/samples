using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JSComponentGeneration.Build.Common
{
    internal static class RazorComponentDescriptorReader
    {
        public static List<RazorComponentDescriptor> ReadWithNamesFromTagHelperCache(string path, ISet<string> componentNames)
        {
            var componentDescriptors = new List<RazorComponentDescriptor>();
            var foundComponents = new HashSet<string>();

            foreach (var tagHelperDescriptor in GenerateTagHelperDescriptors(path))
            {
                if (tagHelperDescriptor.Kind != "Components.Component")
                {
                    continue;
                }

                var componentFullName = tagHelperDescriptor.Name;

                if (foundComponents.Contains(componentFullName))
                {
                    continue;
                }

                var lastIndexOfDot = componentFullName.LastIndexOf('.');
                var componentName = componentFullName.Substring(lastIndexOfDot + 1);

                if (!componentNames.Contains(componentName))
                {
                    continue;
                }

                componentDescriptors.Add(new(tagHelperDescriptor));
                foundComponents.Add(componentFullName);
            }

            return componentDescriptors;
        }

        private static IEnumerable<TagHelperDescriptor> GenerateTagHelperDescriptors(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<IEnumerable<TagHelperDescriptor>>(json);
        }
    }
}
