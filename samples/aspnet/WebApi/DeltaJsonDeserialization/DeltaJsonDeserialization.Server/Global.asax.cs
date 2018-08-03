using System;
using System.Web;
using System.Web.Http;

namespace DeltaJsonDeserialization.Server
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Configure);
        }
    }
}