using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SQLMembership_Identity_OWIN.Startup))]
namespace SQLMembership_Identity_OWIN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
