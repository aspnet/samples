using Microsoft.Owin;
using Owin;
using System.Threading.Tasks;

namespace MyApp
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
