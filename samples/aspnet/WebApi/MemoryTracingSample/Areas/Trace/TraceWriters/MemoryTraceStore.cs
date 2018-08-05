using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Tracing;
using ROOT_PROJECT_NAMESPACE.Areas.Trace.Models;

namespace ROOT_PROJECT_NAMESPACE.Areas.Trace.TraceWriters
{
    /// <summary>
    /// This class maintains an in-memory store of traces captured
    /// by the <see cref="MemoryTraceWriter"/>.
    /// </summary>
    /// <remarks>
    /// This class uses a rolling memory buffer that discards the oldest
    /// traces as needed.
    /// </remarks>
    public class MemoryTraceStore
    {
        private static readonly MemoryTraceStore _instance = new MemoryTraceStore();

        private object _thisLock = new object();
        private int _maxRequests;
        private LinkedList<RequestTrace> _requestList = new LinkedList<RequestTrace>();
        private Dictionary<string, RequestTrace> _requestDictionary = new Dictionary<string, RequestTrace>();

        /// <summary>
        /// Creates a new instance of the <see cref="MemoryTraceStore"/> class.
        /// </summary>
        public MemoryTraceStore()
            : this(100)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="MemoryTraceStore"/> class.
        /// </summary>
        /// <param name="maxRequests">The maximum number of requests to retain
        /// in a rolling memory buffer.</param>
        public MemoryTraceStore(int maxRequests)
        {
            _maxRequests = maxRequests;
        }

        /// <summary>
        /// Gets the application-wide <see cref="MemoryTraceStore"/> singleton.
        /// </summary>
        public static MemoryTraceStore Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets from the store all the current request traces.
        /// </summary>
        /// <returns>The collection of all available traces.</returns>
        public IEnumerable<RequestTrace> GetRequestTraces()
        {
            lock (_thisLock)
            {
                return _requestList.ToArray();
            }
        }

        /// <summary>
        /// Adds a single trace record to the store.
        /// </summary>
        /// <remarks>
        /// All trace records are correlated with their associated
        /// request GUID inside this method.
        /// </remarks>
        /// <param name="traceRecord"></param>
        public void AddTrace(TraceRecord traceRecord)
        {
            lock (_thisLock)
            {
                RequestTrace requestTrace;
                if (_requestList.Count >= _maxRequests)
                {
                    requestTrace = _requestList.Last.Value;
                    _requestList.RemoveLast();
                    _requestDictionary.Remove(requestTrace.Id);
                }

                if (!_requestDictionary.TryGetValue(traceRecord.RequestId.ToString(), out requestTrace))
                {
                    requestTrace = CreateRequestTrace(traceRecord);
                    _requestDictionary[requestTrace.Id] = requestTrace;
                    _requestList.AddFirst(requestTrace);
                }

                TraceItem traceItem = CreateTraceItem(traceRecord);
                requestTrace.Traces.Add(traceItem);
                if (traceItem.Status != 0)
                {
                    requestTrace.Status = traceItem.Status;
                    requestTrace.Reason = ((HttpStatusCode) traceItem.Status).ToString();
                }
            }
        }

        private static RequestTrace CreateRequestTrace(TraceRecord traceRecord)
        {
            HttpRequestMessage request = traceRecord.Request;
            RequestTrace requestTrace = new RequestTrace
            {
                Id = traceRecord.RequestId.ToString(),
                Method = (request == null) ? String.Empty : request.Method.ToString(),
                Uri = (request == null || request.RequestUri == null) ? String.Empty : request.RequestUri.ToString(),
                Timestamp = traceRecord.Timestamp,
                Status = (int) traceRecord.Status,
                Reason = traceRecord.Status == 0 ? null : traceRecord.Status.ToString()
            };

            return requestTrace;
        }

        private static TraceItem CreateTraceItem(TraceRecord traceRecord)
        {
            TraceItem traceItem = new TraceItem
            {
                Category = traceRecord.Category,
                Exception = traceRecord.Exception,
                Kind = traceRecord.Kind,
                Level = traceRecord.Level,
                Message = traceRecord.Message,
                Operator = traceRecord.Operator,
                Operation = traceRecord.Operation,
                Status = (int) traceRecord.Status,
                Timestamp = traceRecord.Timestamp,

            };

            return traceItem;
        }
    }
}
