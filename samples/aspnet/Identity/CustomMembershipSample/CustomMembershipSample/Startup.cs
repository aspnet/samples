using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomMembershipSample;
using Owin;

namespace CustomMembershipSample
{
    // Start up class that configures the OWIN pipelines with the middlewares to use
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}