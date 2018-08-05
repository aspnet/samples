using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCustomServer
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    internal class CustomServer : IDisposable
    {
        internal CustomServer()
        {
            // Create a configurable instance
        }

        internal void Start(AppFunc next, IList<IDictionary<string, object>> addresses)
        {
            // Start a server that listens on the given address(es).

            // Listen for incoming requests.

            // Dispatch them into the OWIN pipeline by calling next.

            // Clean up the request when the AppFunc Task completes.

            // Shut down when Dispose is called.
        }

        public void Dispose()
        {
            // Stop
        }
    }
}
