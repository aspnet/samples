using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace Elmah.Server.ExceptionHandling
{
    public class ElmahExceptionLogger : ExceptionLogger
    {
        private const string HttpContextBaseKey = "MS_HttpContext";

        public override void Log(ExceptionLoggerContext context)
        {
            // Retrieve the current HttpContext instance for this request.
            HttpContext httpContext = GetHttpContext(context.Request);

            if (httpContext == null)
            {
                return;
            }

            // Wrap the exception in an HttpUnhandledException so that ELMAH can capture the original error page.
            Exception exceptionToRaise = new HttpUnhandledException(message: null, innerException: context.Exception);

            // Send the exception to ELMAH (for logging, mailing, filtering, etc.).
            ErrorSignal signal = ErrorSignal.FromContext(httpContext);
            signal.Raise(exceptionToRaise, httpContext);
        }

        private static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            HttpContextBase contextBase = GetHttpContextBase(request);

            if (contextBase == null)
            {
                return null;
            }

            return ToHttpContext(contextBase);
        }

        private static HttpContextBase GetHttpContextBase(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            object value;

            if (!request.Properties.TryGetValue(HttpContextBaseKey, out value))
            {
                return null;
            }

            return value as HttpContextBase;
        }

        private static HttpContext ToHttpContext(HttpContextBase contextBase)
        {
            return contextBase.ApplicationInstance.Context;
        }
    }
}
