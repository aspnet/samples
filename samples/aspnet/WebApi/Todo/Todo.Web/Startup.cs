using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Todo.Web.Startup))]

namespace Todo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
