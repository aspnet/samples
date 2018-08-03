using Microsoft.OData.Edm;
using ODataVersioningSample.Extensions;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ODataVersioningSample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            config.AddODataQueryFilter();

            var controllerSelector = new ODataVersionControllerSelector(config);
            config.Services.Replace(typeof(IHttpControllerSelector), controllerSelector);

            V1.WebApiConfig.Register(config);
            V2.WebApiConfig.Register(config);

            config.EnsureInitialized();
        }
    }
}