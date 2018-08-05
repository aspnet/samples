using Owin;
using System;
using System.Web.Routing;

namespace AspNetRoutes
{
    public class Global : System.Web.HttpApplication
    {
        // How to hook OWIN pipelines into the normal Asp.Net route table side by side with other components.
        protected void Application_Start(object sender, EventArgs e)
        {
            // Paths that start with /owin will be directed to our startup class.
            RouteTable.Routes.MapOwinPath("/owin");

            RouteTable.Routes.MapOwinPath("/special", app =>
            {
                app.Run(OwinApp2.Invoke);
            });
        }
    }
}