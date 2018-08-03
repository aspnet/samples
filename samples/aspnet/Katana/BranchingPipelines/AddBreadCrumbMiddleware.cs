using Microsoft.Owin;
using System.Threading.Tasks;

namespace BranchingPipelines
{
    public class AddBreadCrumbMiddleware : OwinMiddleware
    {
        private string _breadcrumb;

        public AddBreadCrumbMiddleware(OwinMiddleware next, string breadcrumb)
            : base(next)
        {
            _breadcrumb = breadcrumb;
        }

        public override Task Invoke(IOwinContext context)
        {
            context.Request.Headers.Append("breadcrumbs", _breadcrumb);
            return Next.Invoke(context);
        }
    }
}