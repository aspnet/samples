using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ROOT_PROJECT_NAMESPACE.Areas.Trace.Models;
using ROOT_PROJECT_NAMESPACE.Areas.Trace.TraceWriters;

namespace ROOT_PROJECT_NAMESPACE.Areas.Trace.Controllers
{
    /// <summary>
    /// This class is the MVC Controller that controls
    /// views of the In-Memory traces.
    /// </summary>
    public class TraceController : Controller
    {

        /// <summary>
        /// Index action to view In-Memory traces.
        /// </summary>
        /// <remarks>
        /// This page is generally viewed only through the
        /// root URL plus "/Trace".  It accepts optional
        /// arguments to filter the visible traces.
        /// </remarks>
        /// <param name="httpMethod">If not <c>null</c> shows only traces for this HTTP method.</param>
        /// <param name="path">If not <c>null</c> shows only traces whose URL begins with this path.</param>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult Index(string httpMethod = null, string path = null)
        {
            // Filter by query parameters
            List<TraceGroup> traceGroups = GetTraceGroups(httpMethod, path).ToList();

            // Detect when there is only a single request or request group
            // and forward as appropriate to save the user unneeded clicks.
            return (traceGroups.Count == 1)
                ? (traceGroups[0].RequestTraces.Count == 1)
                    ? View("TraceDetails", traceGroups[0].RequestTraces[0])
                    : View("GroupDetails", traceGroups[0].RequestTraces)
                : View(traceGroups);
        }

        /// <summary>
        /// Action to display trace groups
        /// </summary>
        /// <remarks>
        /// A trace group consists of all requests that
        /// have the same URL.  This action accepts optional
        /// arguments to filter which groups to display.
        /// </remarks>
        /// <param name="httpMethod">If not <c>null</c> shows only traces for this HTTP method.</param>
        /// <param name="path">If not <c>null</c> shows only traces whose URL begins with this path.</param>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult GroupDetails(string httpMethod = null, string path = null)
        {
            IEnumerable<RequestTrace> requestTraces = MemoryTraceStore.Instance.GetRequestTraces().Where(r => IsMatch(r, httpMethod, path));
            return View("GroupDetails", requestTraces);
        }

        /// <summary>
        /// Action to display the detail trace records for a single request.
        /// </summary>
        /// <remarks>
        /// A NotFound response will be generated if there is no request
        /// in the current In-Memory buffer with that <paramref name="id"/>.
        /// </remarks>
        /// <param name="id">The unique identifier of the request.  
        /// This is the <see cref="Guid"/> associated with the
        /// request as the traces were captured.</param>
        /// <returns>The <see cref="ActionResult"/>.</returns>
        public ActionResult TraceDetails(string id)
        {
            RequestTrace requestTrace = MemoryTraceStore.Instance.GetRequestTraces().FirstOrDefault(t => id == null || t.Id == id);
            if (requestTrace == null)
            {
                return new HttpNotFoundResult();
            }

            return View("TraceDetails", requestTrace);
        }

        private static string PathFromUri(string uri)
        {
            if (String.IsNullOrEmpty(uri))
            {
                return String.Empty;
            }

            string baseUri = new Uri(uri).PathAndQuery;
            if (baseUri.StartsWith("/"))
            {
                baseUri = baseUri.Substring(1);
            }

            return baseUri;
        }

        private IEnumerable<RequestTrace> GetRequestTraces(string httpMethod, string path)
        {
            List<RequestTrace> requestTraces = new List<RequestTrace>();
            foreach (RequestTrace requestTrace in MemoryTraceStore.Instance.GetRequestTraces())
            {
                if (IsMatch(requestTrace, httpMethod, path))
                {
                    requestTraces.Add(requestTrace);
                }
            }

            return requestTraces;
        }

        private static string EncodedPathFromUri(string uri)
        {
            return PathFromUri(uri).Replace('/', '-');
        }

        private static bool IsMatch(RequestTrace requestTrace, string httpMethod, string path)
        {
            string baseUri = EncodedPathFromUri(requestTrace.Uri);

            return ((httpMethod == null || requestTrace.Method.Equals(httpMethod, StringComparison.OrdinalIgnoreCase)) &&
                    (path == null || baseUri.Equals(path, StringComparison.OrdinalIgnoreCase)));
        }

        private IEnumerable<TraceGroup> GetTraceGroups(string httpMethod, string path)
        {
            Dictionary<string, TraceGroup> traceGroups = new Dictionary<string, TraceGroup>(StringComparer.OrdinalIgnoreCase);

            foreach (RequestTrace requestTrace in GetRequestTraces(httpMethod, path))
            {
                string requestPath = PathFromUri(requestTrace.Uri);
                string key = requestTrace.Method + '-' + requestPath;
                TraceGroup traceGroup = null;
                if (!traceGroups.TryGetValue(key, out traceGroup))
                {
                    traceGroup = new TraceGroup
                    {
                        Uri = requestTrace.Uri,
                        HttpMethod = requestTrace.Method,
                        Path = EncodedPathFromUri(requestTrace.Uri)
                    };

                    traceGroups[key] = traceGroup;
                }

                traceGroup.RequestTraces.Add(requestTrace);
            }

            return traceGroups.Values;
        }
    }
}
