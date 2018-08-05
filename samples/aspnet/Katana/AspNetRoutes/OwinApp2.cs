using System.Threading.Tasks;
using Microsoft.Owin;

namespace AspNetRoutes
{
    public class OwinApp2
    {
        // Invoked once per request.
        public static Task Invoke(IOwinContext context)
        {
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync("Hello World 2");
        }
    }
}