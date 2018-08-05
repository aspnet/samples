using System;
using System.Net.Http;
using System.Web.Http.Tracing;

namespace ROOT_PROJECT_NAMESPACE.Areas.Trace.TraceWriters
{
    /// <summary>
    /// This class is responsible for
    /// </summary>
    public class MemoryTraceWriter : ITraceWriter
    {
        private MemoryTraceStore _traceStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTraceWriter"/> class.
        /// </summary>
        /// <param name="traceStore">The <see cref="TraceStore"/> in which to store traces.</param>
        public MemoryTraceWriter(MemoryTraceStore traceStore)
        {
            if (traceStore == null)
            {
                throw new ArgumentNullException("traceStore");
            }

            _traceStore = traceStore;
        }

        /// <summary>
        /// Captures a single trace.
        /// </summary>
        /// <remarks>
        /// This method is called by framework or user code to record a single trace.
        /// A callback is provided by the caller to allow it to fill in the detailed
        /// information for that trace.
        /// </remarks>
        /// <param name="request">The <see cref="HttpRequestMessage"/> associated with the trace.</param>
        /// <param name="category">The logical category classification for this trace.  It is intended
        /// to allow user or framework code to group functionally related traces under a common name.</param>
        /// <param name="level">The <see cref="TraceLevel"/> of the trace.</param>
        /// <param name="traceAction">The action to invoke to allow the caller to fill-in additional
        /// details in the given <see cref="TraceRecord"/>.</param>
        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }

            if (((int)level < 0) || ((int)level > (int)TraceLevel.Fatal))
            {
                throw new ArgumentOutOfRangeException("level");
            }

            if (request != null && request.RequestUri != null)
            {
                TraceRecord traceRecord = new TraceRecord(request, category, level);
                traceAction(traceRecord);
                _traceStore.AddTrace(traceRecord);
            }
        }
    }
}
