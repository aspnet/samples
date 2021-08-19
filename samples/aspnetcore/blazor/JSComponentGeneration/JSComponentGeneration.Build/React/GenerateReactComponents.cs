using System;
using System.Collections.Generic;
using System.IO;
using JSComponentGeneration.Build.Common;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace JSComponentGeneration.Build.React
{
    public class GenerateReactComponents : Task
    {
        private const string BlazorHelperFile = "blazor-react.js";

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
                componentNames = new(RazorComponentReader.ReadWithAttributeFromAssembly(assemblyFilePath, "GenerateReactAttribute"));
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

            var blazorHelperDestinationPath = $"{JavaScriptComponentOutputDirectory}/{BlazorHelperFile}";

            if (!File.Exists(blazorHelperDestinationPath))
            {
                try
                {
                    var blazorComponentSourcePath = $"{OutputPath}/js/{BlazorHelperFile}";
                    File.Copy(blazorComponentSourcePath, blazorHelperDestinationPath);
                }
                catch (Exception e)
                {
                    Log.LogError($"Could not copy the '{BlazorHelperFile}' file: {e.Message}");
                    return false;
                }
            }

            foreach (var componentDescriptor in componentDescriptors)
            {
                try
                {
                    ReactComponentWriter.Write(JavaScriptComponentOutputDirectory, componentDescriptor);
                }
                catch (Exception e)
                {
                    Log.LogError($"Could not write a React component from Razor component '{componentDescriptor.Name}': {e.Message}");
                    return false;
                }
            }

            Log.LogMessage("React component generation complete.");
            return true;
        }
    }
}
