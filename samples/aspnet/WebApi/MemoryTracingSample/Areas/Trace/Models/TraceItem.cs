using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Tracing;
using System.Net;

namespace ROOT_PROJECT_NAMESPACE.Areas.Trace.Models
{
    /// <summary>
    /// Class that describes a single trace record.
    /// </summary>
    public class TraceItem
    {
        public DateTime Timestamp { get; set; }

        public TraceLevel Level { get; set; }

        public string Category { get; set; }

        public TraceKind Kind { get; set; }

        public string Operator { get; set; }

        public string Operation { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public int Status { get; set; }
    }
}
