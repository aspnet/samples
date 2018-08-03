using System.Data.Entity;
using System.Web.Http;
using System.Web.Routing;
using ODataBatchSample.Models;

namespace ODataBatchSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            Database.SetInitializer<CustomersContext>(new CustomersContextInitializer());
        }
    }
}
