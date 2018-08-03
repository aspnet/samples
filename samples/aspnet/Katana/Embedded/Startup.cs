using Owin;
using Microsoft.Owin;
using System.Threading.Tasks;

namespace Embedded
{
    public class Startup
    {
        // Invoked once at startup to configure your application.
        public void Configuration(IAppBuilder app)
        {
            app.UseWelcomePage();
        }
    }
}
