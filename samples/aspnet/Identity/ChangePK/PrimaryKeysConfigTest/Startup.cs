using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrimaryKeysConfigTest.Startup))]
namespace PrimaryKeysConfigTest
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
