using System.Web.Http;
using System.Web.Mvc;

namespace ROOT_PROJECT_NAMESPACE.Areas.Trace
{
    /// <summary>
    /// This class exists to register the In-Memory tracing feature.
    /// It is called during application start-up.
    /// </summary>
    public class TraceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Trace";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Trace_default",
                "Trace/{action}/{id}",
                new { controller = "Trace", action = "Index", id = UrlParameter.Optional }
            );

            TraceConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}
