using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UnitTestAccountController.Startup))]
namespace UnitTestAccountController
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
