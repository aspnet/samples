using Microsoft.OData.Client;
using System;

namespace ODataCompositeKeySample.Tests.Default
{
    public partial class Container
    {
        public Container()
            : this(new Uri("http://localhost:33051"))
        {
            this.SendingRequest2 += Container_SendingRequest;
        }

        void Container_SendingRequest(object sender, SendingRequest2EventArgs e)
        {
            Console.WriteLine("{0} {1}", e.RequestMessage.Method, e.RequestMessage.Url);
        }
    }
}
