using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using JSComponentGeneration.Build.Common;
using JSComponentGeneration.Shared;

namespace JSComponentGeneration.Build.Angular
{
    internal static class AngularComponentWriter
    {
        private const string EventCallbackTypeName = "Microsoft.AspNetCore.Components.EventCallback";

        private const string AngularComponentTemplate =
@"import {{ Component, EventEmitter, Input, Output }} from '@angular/core';
import {{ BlazorAdapterComponent }} from '../blazor-adapter/blazor-adapter.component';

@Component({{
  selector: '{0}',
  template: '',
}})

export class {1}Component extends BlazorAdapterComponent {{{2}
}}
";

        public static void Write(string outputDirectory, RazorComponentDescriptor componentDescriptor)
        {
            var jsTypeNames = componentDescriptor.Parameters
                .Select(p => GetJavaScriptTypeName(p.TypeName))
                .ToArray();

            var angularComponentName = CasingUtilities.ToKebabCase(componentDescriptor.Name);
            var propertyList = GetPropertyList(componentDescriptor.Parameters, jsTypeNames);
            var componentContents = string.Format(AngularComponentTemplate, angularComponentName, componentDescriptor.Name, propertyList);

            var angularComponentFolder = $"{outputDirectory}/{angularComponentName}";
            var angularComponentFile = $"{angularComponentFolder}/{angularComponentName}.component.ts";

            Directory.CreateDirectory(angularComponentFolder);
            File.WriteAllText(angularComponentFile, componentContents);
        }

        private static string GetPropertyList(IReadOnlyList<BoundAttributeDescriptor> parameters, string[] jsTypeNames)
        {
            Debug.Assert(parameters.Count == jsTypeNames.Length);

            var stringBuilder = new StringBuilder();

            for (var i = 0; i < parameters.Count; i++)
            {
                var camelCaseName = CasingUtilities.PascalToCamelCase(parameters[i].Name);
                var typeName = jsTypeNames[i];

                if (typeName == "eventcallback")
                {
                    stringBuilder.Append("\n  @Output() ");
                    stringBuilder.Append(camelCaseName);
                    stringBuilder.Append(": EventEmitter<any> = new EventEmitter();");
                }
                else
                {
                    stringBuilder.Append("\n  @Input() ");
                    stringBuilder.Append(camelCaseName);
                    stringBuilder.Append(": ");
                    stringBuilder.Append(typeName);
                    stringBuilder.Append(" | null = null;");
                }
            }

            return stringBuilder.ToString();
        }

        private static string GetJavaScriptTypeName(string cSharpTypeName)
            => cSharpTypeName switch
            {
                var x when x == typeof(byte).FullName => "number",
                var x when x == typeof(sbyte).FullName => "number",
                var x when x == typeof(int).FullName => "number",
                var x when x == typeof(uint).FullName => "number",
                var x when x == typeof(short).FullName => "number",
                var x when x == typeof(ushort).FullName => "number",
                var x when x == typeof(long).FullName => "number",
                var x when x == typeof(ulong).FullName => "number",
                var x when x == typeof(float).FullName => "number",
                var x when x == typeof(double).FullName => "number",
                var x when x == typeof(decimal).FullName => "number",
                var x when x == typeof(bool).FullName => "boolean",
                var x when x == typeof(char).FullName => "string",
                var x when x == typeof(string).FullName => "string",
                var x when x.StartsWith(EventCallbackTypeName) => "eventcallback",
                _ => "object"
            };
    }
}
