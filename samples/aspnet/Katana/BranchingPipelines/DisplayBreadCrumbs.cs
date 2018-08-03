using Microsoft.Owin;
using System.Threading.Tasks;

namespace BranchingPipelines
{
    public class DisplayBreadCrumbs : OwinMiddleware
    {
        public DisplayBreadCrumbs(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            context.Response.ContentType = "text/plain";

            string responseText = context.Request.Headers.Get("breadcrumbs") + "\r\n"
                + "PathBase: " + context.Request.PathBase + "\r\n"
                + "Path: " + context.Request.Path + "\r\n";

            return context.Response.WriteAsync(responseText);
        }
    }
}