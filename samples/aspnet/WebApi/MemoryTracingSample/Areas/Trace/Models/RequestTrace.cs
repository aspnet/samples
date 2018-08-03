using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ROOT_PROJECT_NAMESPACE.Areas.Trace.Models
{
    /// <summary>
    /// Class that describes a single HTTP request.
    /// It holds a collection of all the traces that
    /// occurred for that single request.
    /// </summary>
    public class RequestTrace
    {
        private List<TraceItem> _traces = new List<TraceItem>();

        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Uri { get; set; }

        public string Method { get; set; }

        public int Status { get; set; }

        public string Reason { get; set; }

        public IList<TraceItem> Traces 
        { 
            get { return _traces; } 
        }
    }
}
