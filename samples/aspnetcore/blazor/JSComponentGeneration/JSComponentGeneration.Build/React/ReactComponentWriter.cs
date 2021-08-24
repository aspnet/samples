using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using JSComponentGeneration.Build.Common;
using JSComponentGeneration.Shared;

namespace JSComponentGeneration.Build.React
{
    internal class ReactComponentWriter
    {
        private const string ReactComponentTemplate =
@"import {{ useBlazor }} from './blazor-react';

export function {0}({{{1}
}}) {{
  const fragment = useBlazor('{2}', {{{3}
  }});

  return fragment;
}}
";

        public static void Write(string outputDirectory, RazorComponentDescriptor componentDescriptor)
        {
            var identifierName = CasingUtilities.ToKebabCase(componentDescriptor.Name);
            var componentParameterList = GetParameterList(componentDescriptor.Parameters, 1);
            var useBlazorParameterList = GetParameterList(componentDescriptor.Parameters, 2);
            var componentContents = string.Format(
                CultureInfo.InvariantCulture,
                ReactComponentTemplate,
                componentDescriptor.Name,
                componentParameterList,
                identifierName,
                useBlazorParameterList);

            var reactComponentFile = $"{outputDirectory}/{componentDescriptor.Name}.js";

            File.WriteAllText(reactComponentFile, componentContents);
        }

        private static string GetParameterList(IReadOnlyList<BoundAttributeDescriptor> parameters, int indentLevel)
        {
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < parameters.Count; i++)
            {
                var camelCaseName = CasingUtilities.PascalToCamelCase(parameters[i].Name);

                stringBuilder.AppendLine();

                for (var j = 0; j < indentLevel; j++)
                {
                    stringBuilder.Append("  ");
                }

                stringBuilder.Append(camelCaseName);
                stringBuilder.Append(',');
            }

            return stringBuilder.ToString();
        }
    }
}
