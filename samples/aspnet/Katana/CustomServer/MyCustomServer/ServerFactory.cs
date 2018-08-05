using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCustomServer
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public static class OwinServerFactory
    {
        /// <summary>
        /// Optional. This gives the server the chance to tell the application about what capabilities are supported.
        /// </summary>
        /// <param name="properties"></param>
        public static void Initialize(IDictionary<string, object> properties)
        {
            // TODO: Add Owin.Types.BuilderProperties for setting capabilities, etc..


            // Consider adding a configurable object to the properties if the application needs to set some specific server settings.
            properties[typeof(CustomServer).FullName] = new CustomServer();
        }

        public static IDisposable Create(AppFunc app, IDictionary<string, object> properties)
        {
            object obj;

            // Get the user configured server instance, if any.
            CustomServer server = null;
            if (properties.TryGetValue(typeof(CustomServer).FullName, out obj))
            {
                server = obj as CustomServer;
            }
            server = server ?? new CustomServer();

            // Get the address collection
            IList<IDictionary<string, object>> addresses = null;
            if (properties.TryGetValue("host.Addresses", out obj))
            {
                addresses = obj as IList<IDictionary<string, object>>;
            }

            server.Start(app, addresses);

            return server;
        }
    }
}
