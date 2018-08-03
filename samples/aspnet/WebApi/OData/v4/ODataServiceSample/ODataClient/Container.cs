using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using System;
using System.Xml;

namespace ODataClient
{
    /// <summary>
    /// Create an override for the generated Container (i.e. container) for the ODataService.Sample
    /// that displays the type of the method and the URL of requests. 
    /// </summary>
    public partial class Container
    {
        public Container()
            : this(new Uri("http://localhost:50231"))
        {
            SendingRequest2 += Container_SendingRequest2;
        }
        private void Container_SendingRequest2(object sender, SendingRequest2EventArgs e)
        {
            Console.WriteLine("\t{0} {1}", e.RequestMessage.Method, e.RequestMessage.Url.ToString());
        }
    }
}
