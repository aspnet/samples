using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UniversalProviders_Identity_Migrations.Startup))]
namespace UniversalProviders_Identity_Migrations
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
