using JSComponentGeneration.Build.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace JSComponentGeneration.Build.Angular
{
    public class GenerateAngularComponents : Task
    {
        private const string BlazorAdapterComponentName = "blazor-adapter";

        [Required]
        public string OutputPath { get; set; }

        [Required]
        public string IntermediateOutputPath { get; set; }

        [Required]
        public string AssemblyName { get; set; }

        [Required]
        public string JavaScriptComponentOutputDirectory { get; set; }

        public override bool Execute()
        {
            var assemblyFilePath = $"{OutputPath}/{AssemblyName}.dll";
            HashSet<string> componentNames;

            try
            {
                componentNames = new(RazorComponentReader.ReadWithAttributeFromAssembly(assemblyFilePath, "GenerateAngularAttribute"));
            }
            catch (Exception e)
            {
                Log.LogError($"An exception occurred while reading the specified assembly: {e.Message}");
                return false;
            }

            var tagHelperCacheFileName = $"{IntermediateOutputPath}/{AssemblyName}.TagHelpers.output.cache";
            List<RazorComponentDescriptor> componentDescriptors;

            try
            {
                componentDescriptors = RazorComponentDescriptorReader.ReadWithNamesFromTagHelperCache(tagHelperCacheFileName, componentNames);
            }
            catch (Exception e)
            {
                Log.LogError($"An exception occurred while reading the tag helper output cache: {e.Message}");
                return false;
            }

            var blazorComponentFile = $"{BlazorAdapterComponentName}.component.ts";
            var blazorComponentDestinationFolder = $"{JavaScriptComponentOutputDirectory}/{BlazorAdapterComponentName}";
            var blazorComponentDestinationPath = $"{blazorComponentDestinationFolder}/{blazorComponentFile}";

            if (!File.Exists(blazorComponentDestinationPath))
            {
                try
                {
                    var blazorComponentSourcePath = $"{OutputPath}/js/{blazorComponentFile}";
                    Directory.CreateDirectory(blazorComponentDestinationFolder);
                    File.Copy(blazorComponentSourcePath, blazorComponentDestinationPath);
                }
                catch (Exception e)
                {
                    Log.LogError($"Could not copy the Blazor adapter component: {e.Message}");
                    return false;
                }
            }

            foreach (var componentDescriptor in componentDescriptors)
            {
                try
                {
                    AngularComponentWriter.Write(JavaScriptComponentOutputDirectory, componentDescriptor);
                }
                catch (Exception e)
                {
                    Log.LogError($"Could not write an Angular component from Razor component '{componentDescriptor.Name}': {e.Message}");
                    return false;
                }
            }

            Log.LogMessage("Angular component generation complete.");
            return true;
        }
    }
}
