using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace WebApiEtwTracing
{
    /// <summary>
    /// Implementation of <see cref="ITraceWriter"/> that traces to ETW.
    /// </summary>
    public class WebApiEtwTraceWriter : ITraceWriter
    {
        // Duplicates of internal constants in HttpError
        private const string MessageKey = "Message";
        private const string MessageDetailKey = "MessageDetail";
        private const string ModelStateKey = "ModelState";
        private const string ExceptionMessageKey = "ExceptionMessage";
        private const string ExceptionTypeKey = "ExceptionType";
        private const string StackTraceKey = "StackTrace";
        private const string InnerExceptionKey = "InnerException";

        /// <summary>
        /// Instantiates an instance of the <see cref="WebApiEtwTraceWriter"/> class.
        /// </summary>
        public WebApiEtwTraceWriter()
        {
            // Setup the default pre-filter to accept only trace requests that are:
            //  1. Warn, Error or Critical level, or
            //  2. Info level in the 2 known categories used for the incoming request,
            //     outgoing response, or controller action handling.
            TraceRequestFilter = (level, category) =>
                level >= TraceLevel.Warn ||
                (level >= TraceLevel.Info &&
                    (String.Equals(category, TraceCategories.RequestCategory, StringComparison.Ordinal) ||
                    String.Equals(category, TraceCategories.ActionCategory, StringComparison.Ordinal)));

            // Setup the final record filter to trace only those records that:
            //  1. Are Warn, Error or Critical level, or
            //  2. Are the incoming request-received or outgoing sending-response category, or
            //  3. Are the specific InvokeActionAsync operation used to invoke the controller's action
            TraceRecordFilter = (record) =>
                record.Level >= TraceLevel.Warn ||
                String.Equals(record.Category, TraceCategories.RequestCategory, StringComparison.Ordinal) || 
                String.Equals(record.Operation, "InvokeActionAsync", StringComparison.Ordinal);
        }

        /// <summary>
        /// Gets or sets a predicate to determine whether an individual trace
        /// request should be accepted.
        /// </summary>
        /// <value>
        /// The default value for this predicate allows only a subset of traces
        /// to be accepted, including the incoming request, outgoing response,
        /// and any warnings or errors.  If this predicate returns <c>true</c> the
        /// trace request will be accepted, otherwise it will be discarded.
        /// A <c>null</c> value for this predicate allows all trace requests.
        /// </value>
        public Func<TraceLevel, string, bool> TraceRequestFilter { get; set; }

        /// <summary>
        /// Gets or sets a predicate to determine whether a specific <see cref="TraceRecord"/>
        /// should be written to ETW.
        /// </summary>
        /// <value>
        /// This predicate is executed only after the <see cref="TraceRequestFilter"/>
        /// predicate has returned <c>true</c> to accept the trace request.
        /// This predicate can examine the fully-formed <see cref="TraceRecord"/> to determine
        /// whether it should be written to ETW.  If the predicate returns <c>true</c> the 
        /// <see cref="TraceRecord"/> will be written to ETW, otherwise it will be discarded.
        /// A <c>null</c> value for this predicate allows all <see cref="TraceRecord"/> instances to be written.
        /// </value>
        public Func<TraceRecord, bool> TraceRecordFilter { get; set; }

        /// <summary>
        /// Traces the specified information if permitted.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/> to associate with this trace.</param>
        /// <param name="category">A string specifying an arbitrary category name for the trace.</param>
        /// <param name="level">The <see cref="TraceLevel"/> of the trace.</param>
        /// <param name="traceAction">An action to call for the detailed trace information if this
        /// method determines the trace request will be honored.  The action is expected to fill
        /// in the given <see cref="TraceRecord"/> with any information is wishes.</param>
        /// <remarks>
        /// This is the public entry point for tracing via <see cref="ITraceWriter"/>, and it
        /// is called by both core WebAPI code as well as user code.  This implementation will
        /// use <see cref="TraceRequestFilter"/> and <see cref="TraceRecordFilter"/> to determine
        /// whether to trace the information or discard it.
        /// </remarks>
        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            // Any trace request that does not pass our pre-filter is discarded.
            // This bypasses record instantiation and string formatting by the
            // trace caller.
            if (TraceRequestFilter != null && !TraceRequestFilter(level, category))
            {
                return;
            }

            // Invoke the caller's action to get the detailed information to trace.
            TraceRecord record = new TraceRecord(request, category, level);
            traceAction(record);

            // Convert all known WebAPI user error responses to a friendlier message.
            // This will not alter genuine InternalServerError responses due to an unexpected error.
            TranslateHttpResponseException(record);
            level = record.Level;

            // Any record not passing our post-filter is discarded.
            if (TraceRecordFilter != null && !TraceRecordFilter(record))
            {
                return;
            }

            // Propagate the activity ID we have assigned to this request
            // so that other related activities can correlate to this request.
            ActivityId.SetActivityId(record.RequestId);

            // We use the level and category to select the right ETW event to trace.
            if (level >= TraceLevel.Error)
            {
                // A Critical or Error trace for any category uses a common
                // error-level ETW event.
                WebApiEtwEventSource.Log.Error(
                    record.Operator,
                    record.Operation,
                    record.Exception != null ? record.Exception.ToString() : record.Message);
            }
            else if (level == TraceLevel.Warn)
            {
                // Warning level traces for any category use a common warn-level ETW event.
                WebApiEtwEventSource.Log.Warning(
                    record.Operator,
                    record.Operation,
                    record.Exception != null ? record.Exception.ToString() : record.Message);
            }
            else if (category == TraceCategories.RequestCategory)
            {
                // Traces made for the incoming Http request use a special ETW "Receive" event
                if (record.Kind == TraceKind.Begin)
                {
                    WebApiEtwEventSource.Log.Request(
                        request.RequestUri != null ? request.RequestUri.ToString() : string.Empty,
                        request.Method.ToString()
                        );
                }
                else
                {
                    // Traces made for the outgoing Http response use a special ETW "Reply" event
                    WebApiEtwEventSource.Log.Response(
                        request.RequestUri != null ? request.RequestUri.ToString() : string.Empty,
                        request.Method.ToString(),
                        (int) record.Status,
                        record.Status != 0 ? record.Status.ToString() : string.Empty
                        );
                }
            }
            else
            {
                // All other traces are of the form Begin, End or Trace,
                // and each uses a unique ETW event
                switch (record.Kind)
                {
                    case TraceKind.Begin:
                        WebApiEtwEventSource.Log.OpBegin(
                            record.Operator ?? String.Empty,
                            record.Operation ?? String.Empty,
                            record.Message ?? String.Empty
                            );
                        return;

                    case TraceKind.End:
                        WebApiEtwEventSource.Log.OpEnd(
                            record.Operator ?? String.Empty,
                            record.Operation ?? String.Empty,
                            record.Message ?? String.Empty
                            );
                        return;

                    default:
                        WebApiEtwEventSource.Log.OpTrace(
                            record.Operator ?? String.Empty,
                            record.Operation ?? String.Empty,
                            record.Message ?? String.Empty
                            );
                        return;
                }
            }
        }

        /// <summary>
        /// If the given <see cref="TraceRecord"/> contains an <see cref="HttpResponseException"/>,
        /// this method unwraps the information inside into a more human-readable form for tracing.
        /// </summary>
        /// <param name="traceRecord">The <see cref="TraceRecord"/> to examine and optionally modify.</param>
        /// <remarks>
        /// The WebAPI stack uses <see cref="HttpResponseException"/> to emit an 
        /// <see cref="HttpResponseMessage"/> due to an recoverable error condition,
        /// such as an invalid user request.
        /// </remarks>
        private static void TranslateHttpResponseException(TraceRecord traceRecord)
        {
            HttpResponseException httpResponseException = traceRecord.Exception as HttpResponseException;
            if (httpResponseException != null)
            {
                HttpResponseMessage response = httpResponseException.Response;

                // If the status has been set already, do not overwrite it,
                // otherwise propagate the status into the record.
                if (traceRecord.Status == 0)
                {
                    traceRecord.Status = response.StatusCode;
                }

                // Client level errors are downgraded to TraceLevel.Warn
                if ((int)response.StatusCode < (int)HttpStatusCode.InternalServerError)
                {
                    traceRecord.Level = TraceLevel.Warn;
                }

                // HttpResponseExceptions often contain HttpError instances that carry
                // detailed information that may be filtered out by IncludeErrorDetailPolicy
                // before reaching the client. Capture it here for the trace.
                ObjectContent objectContent = response.Content as ObjectContent;
                if (objectContent != null)
                {
                    HttpError httpError = objectContent.Value as HttpError;
                    if (httpError != null)
                    {
                        // Having a structured HttpError replaces the normal
                        // exception in the record because it has more user-directed
                        // information.
                        traceRecord.Exception = null;

                        object messageObject = null;
                        object messageDetailsObject = null;

                        List<string> messages = new List<string>();

                        if (httpError.TryGetValue(MessageKey, out messageObject))
                        {
                            messages.Add(String.Format(SRResources.HttpErrorUserMessageFormat, messageObject));
                        }

                        if (httpError.TryGetValue(MessageDetailKey, out messageDetailsObject))
                        {
                            messages.Add(String.Format(SRResources.HttpErrorMessageDetailFormat, messageDetailsObject));
                        }

                        // Extract the exception from this HttpError and then incrementally
                        // walk down all inner exceptions.
                        AddExceptions(httpError, messages);

                        // ModelState errors are handled with a nested HttpError
                        object modelStateErrorObject = null;
                        if (httpError.TryGetValue(ModelStateKey, out modelStateErrorObject))
                        {
                            HttpError modelStateError = modelStateErrorObject as HttpError;
                            if (modelStateError != null)
                            {
                                messages.Add(FormatModelStateErrors(modelStateError));
                            }
                        }

                        traceRecord.Message = String.Join(", ", messages);
                    }
                }
            }
        }

        /// <summary>
        /// Unwraps an arbitrarily deep collection of inner exceptions inside an
        /// <see cref="HttpError"/> instance into a list of error messages.
        /// </summary>
        /// <param name="httpError">The input <see cref="HttpError"/>.</param>
        /// <param name="messages">The list of messages to which the exceptions should be added.</param>
        private static void AddExceptions(HttpError httpError, List<string> messages)
        {
            object exceptionMessageObject = null;
            object exceptionTypeObject = null;
            object stackTraceObject = null;
            object innerExceptionObject = null;

            for (int i = 0; httpError != null; i++)
            {
                // For uniqueness, key names append the depth of inner exception
                string indexText = i == 0 ? String.Empty : String.Format("[{0}]", i);

                if (httpError.TryGetValue(ExceptionTypeKey, out exceptionTypeObject))
                {
                    messages.Add(String.Format(SRResources.HttpErrorExceptionTypeFormat, indexText, exceptionTypeObject));
                }

                if (httpError.TryGetValue(ExceptionMessageKey, out exceptionMessageObject))
                {
                    messages.Add(String.Format(SRResources.HttpErrorExceptionMessageFormat, indexText, exceptionMessageObject));
                }

                if (httpError.TryGetValue(StackTraceKey, out stackTraceObject))
                {
                    messages.Add(String.Format(SRResources.HttpErrorStackTraceFormat, indexText, stackTraceObject));
                }

                if (!httpError.TryGetValue(InnerExceptionKey, out innerExceptionObject))
                {
                    break;
                }

                httpError = innerExceptionObject as HttpError;
            }
        }

        /// <summary>
        /// Unwrap model binding errors into human-readable error messages.
        /// </summary>
        /// <param name="httpError">The input <see cref="HttpError"/> to be
        /// treated as a set of model binding errors.</param>
        /// <returns></returns>
        private static string FormatModelStateErrors(HttpError httpError)
        {
            List<string> messages = new List<string>();
            foreach (var pair in httpError)
            {
                IEnumerable<string> errorList = pair.Value as IEnumerable<string>;
                if (errorList != null)
                {
                    messages.Add(String.Format(SRResources.HttpErrorModelStatePairFormat, pair.Key, String.Join(", ", errorList)));
                }
            }

            return String.Format(SRResources.HttpErrorModelStateErrorFormat, String.Join(", ", messages));
        }
    }
}
