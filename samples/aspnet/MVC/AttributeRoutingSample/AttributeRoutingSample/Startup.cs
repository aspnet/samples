
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AttributeRoutingSample.Startup))]
namespace AttributeRoutingSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
