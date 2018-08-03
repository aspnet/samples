using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace CustomAssemblyResolver
{
    /// <summary>
    /// Custom <see cref="IAssembliesResolver"/> which takes the default implementation providing the default 
    /// list of assemblies and then adds the ControllerLibrary assembly as well.
    /// </summary>
    class CustomAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            ICollection<Assembly> baseAssemblies = base.GetAssemblies();
            List<Assembly> assemblies = new List<Assembly>(baseAssemblies);

            // Add our controller library assembly
            try
            {
                Assembly controllerLibraryAssembly = Assembly.LoadFrom(@"ControllerLibrary.dll");
                assemblies.Add(controllerLibraryAssembly);
            }
            catch
            {
                // We ignore errors and just continue
            }

            return assemblies;
        }
    }
}
