using PrimaryKeysConfigTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PrimaryKeysConfigTest
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create Admin role and user
            CreateAdminRole();
        }

        private void CreateAdminRole()
        {
            var roleManager = new RoleManager<CustomRole, int>(new CustomRoleStore(new ApplicationDbContext()));
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create<CustomRole, int>(new CustomRole("Admin"));
            }
        }
    }
}