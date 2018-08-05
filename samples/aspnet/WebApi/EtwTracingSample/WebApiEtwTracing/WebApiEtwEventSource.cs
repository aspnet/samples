using System.Diagnostics.Tracing;

namespace WebApiEtwTracing
{
    /// <summary>
    /// Implementation of <see cref="EventSource"/> used by <see cref="WebApiEtwTraceWriter"/>
    /// to emit ETW events.
    /// </summary>
    [EventSource(Name = "WebApi", 
                 Guid = "{fc7bbb67-1b01-557b-5e0e-c1c4e40b6a24}",
                 LocalizationResources = "WebApiEtwTracing.SRResources")]
    public class WebApiEtwEventSource : EventSource
    {
        /// <summary>
        /// Returns a singleton instance of this class.
        /// </summary>
        /// <remarks>
        /// This is the convention used by other <see cref="EventSource"/> implementations.
        /// </remarks>
        public static WebApiEtwEventSource Log = new WebApiEtwEventSource();

        /// <summary>
        /// Emit an ETW event for an incoming Http request.
        /// </summary>
        /// <param name="uri">The request Uri.</param>
        /// <param name="method">The request method, such as "Get".</param>
        [Event(1, 
                Level = EventLevel.Informational, 
                Opcode = EventOpcode.Receive, 
                Keywords = Keywords.Diagnostic,
                Task = Tasks.Request)]
        public void Request(string uri, string method)
        {
            WriteEvent(1, uri, method);
        }

        /// <summary>
        /// Emit an ETW event for an outgoing Http response.
        /// </summary>
        /// <param name="uri">The request Uri.</param>
        /// <param name="method">The request method, such as "Get".</param>
        /// <param name="status">The string representation of the <see cref="System.Net.Http.HttpStatusCode"/> for the response.</param>
        [Event(2, 
                Level = EventLevel.Informational, 
                Opcode = EventOpcode.Reply,
                Keywords = Keywords.Diagnostic,
                Task = Tasks.Response)]
        public void Response(string uri, string method, int statusCode, string status)
        {
            WriteEvent(2, uri, method, statusCode, status);
        }

        /// <summary>
        /// Emit an ETW event identifying the start of some operation.
        /// </summary>
        /// <param name="name">The name of the object performing the operation, usually the type name.</param>
        /// <param name="operation">The name of the operation, usually the method name.</param>
        /// <param name="message">An optional message.</param>
        [Event(3, 
                Level = EventLevel.Informational, 
                Opcode = EventOpcode.Start,
                Keywords = Keywords.Diagnostic,
                Task = Tasks.OpBegin)]
        public void OpBegin(string name, string operation, string message)
        {
            WriteEvent(3, name, operation, message);
        }

        /// <summary>
        /// Emit an ETW event identifying the end of some operation.
        /// </summary>
        /// <param name="name">The name of the object performing the operation, usually the type name.</param>
        /// <param name="operation">The name of the operation, usually the method name.</param>
        /// <param name="message">An optional message, usually describing the result.</param>
        [Event(4, 
                Level = EventLevel.Informational, 
                Opcode = EventOpcode.Stop,
                Keywords = Keywords.Diagnostic,
                Task = Tasks.OpEnd)]
        public void OpEnd(string name, string operation, string message)
        {
            WriteEvent(4, name, operation, message);
        }

        /// <summary>
        /// Emit an ETW event with a descriptive message.
        /// </summary>
        /// <param name="name">The name of the object performing the operation, usually the type name.</param>
        /// <param name="operation">The name of the operation, usually the method name.</param>
        /// <param name="message">A descriptive message.</param>
        /// <remarks>
        /// This method would be invoked only by user code invoking
        /// <see cref="ITraceWriter.Trace()"/> with <see cref="TraceKind.Trace"/>.
        /// The WebAPI stack does not normally use this entry point.
        /// </remarks>
        [Event(5,
        Level = EventLevel.Informational,
        Opcode = EventOpcode.Info,
        Keywords = Keywords.Diagnostic,
        Task = Tasks.OpTrace)]
        public void OpTrace(string name, string operation, string message)
        {
            WriteEvent(5, name, operation, message);
        }

        /// <summary>
        /// Emit a warning level ETW event.
        /// </summary>
        /// <param name="name">The name of the object performing the operation, usually the type name.</param>
        /// <param name="operation">The name of the operation, usually the method name.</param>
        /// <param name="message">A descriptive message.</param>
        [Event(6, 
                Level = EventLevel.Warning, 
                Opcode = EventOpcode.Info,
                Keywords = Keywords.Diagnostic,
                Task = Tasks.Warning)]
        public void Warning(string name, string operation, string message)
        {
            WriteEvent(6, name, operation, message);
        }

        /// <summary>
        /// Emit an error level ETW event.
        /// </summary>
        /// <param name="name">The name of the object performing the operation, usually the type name.</param>
        /// <param name="operation">The name of the operation, usually the method name.</param>
        /// <param name="message">A descriptive message.</param>
        [Event(7, 
                Level = EventLevel.Error,
                Opcode = EventOpcode.Info,
                Keywords = Keywords.Diagnostic,
                Task = Tasks.Error)]
        public void Error(string name, string operation, string message)
        {
            WriteEvent(7, name, operation, message);
        }

        /// <summary>
        /// Simple enum wrapper for the possible keyword values
        /// attached to the ETW events.
        /// </summary>
        public class Keywords
        {
            /// <summary>
            /// Identifies an ETW trace as diagnostic information.
            /// </summary>
            public const EventKeywords Diagnostic = (EventKeywords)1;
        }

        /// <summary>
        /// Simple enum wrapper for possible Task values
        /// </summary>
        public class Tasks
        {
            public const EventTask Request =  (EventTask)1;
            public const EventTask Response = (EventTask)2;
            public const EventTask OpBegin =  (EventTask)3;
            public const EventTask OpEnd =    (EventTask)4;
            public const EventTask OpTrace =  (EventTask)5;
            public const EventTask Warning =  (EventTask)6;
            public const EventTask Error =    (EventTask)7;
        }
    }
}
