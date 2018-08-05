using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Identity_PasswordPolicy.Startup))]
namespace Identity_PasswordPolicy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
