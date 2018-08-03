using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SingleSignOutSample.Startup))]
namespace SingleSignOutSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
