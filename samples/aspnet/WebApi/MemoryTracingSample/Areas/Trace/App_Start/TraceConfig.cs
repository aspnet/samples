using System.Web.Http;
using System.Web.Http.Tracing;
using ROOT_PROJECT_NAMESPACE.Areas.Trace.TraceWriters;

namespace ROOT_PROJECT_NAMESPACE.Areas.Trace
{
    /// <summary>
    /// This class is responsible for configuring and registering
    /// the In-Memory trace writer.  
    /// It is invoked automatically during start-up.
    /// </summary>
    public static class TraceConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // This code registers the In-memory trace writer using
            // a memory-based store.  To view what is in the store
            // browse to the app's root URL + "/Trace"

            ITraceWriter traceWriter = new MemoryTraceWriter(MemoryTraceStore.Instance);
            config.Services.Replace(typeof(ITraceWriter), traceWriter);
        }
    }
}
