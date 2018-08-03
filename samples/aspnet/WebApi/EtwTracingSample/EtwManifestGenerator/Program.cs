using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EtwManifestGenerator
{
    /// <summary>
    /// This console app is a stand-alone tool that produces a manifest file describing
    /// an ETW event source.  It is used to generate the .man file visible in the solution.
    /// For more details, refer to the solution ReadMe.txt.
    /// </summary>
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: EtwManifestGenerator.exe eventSourceAssemblyName outputManifestName");
                Environment.Exit(1);
            }

            string fileName = Path.GetFullPath(args[0]);
            string outputFileName = Path.GetFullPath(args[1]);

            Assembly assembly = Assembly.LoadFrom(fileName);
            Type eventSource = null;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.BaseType != null && (type.BaseType.Name == "EventSource" || type.BaseType.Name == "EventProviderBase"))
                {
                    eventSource = type;
                    break;
                }
            }

            Console.WriteLine("Generating manifest for {0} into {1}", eventSource.Name, outputFileName);
            bool success = false;
            try
            {
                string manifestContent = EventSource.GenerateManifest(eventSource, fileName);
                if (File.Exists(outputFileName))
                {
                    File.Delete(outputFileName);
                }

                File.WriteAllText(outputFileName, manifestContent);
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:" + ex.Message);
            }

            if (!success)
            {
                Environment.Exit(1);
            }
        }
    }
}
