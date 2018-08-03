using System;

namespace ODataCompositeKeySample.Tests.PeopleService
{
    public partial class Container
    {
        public Container()
            : this(new Uri("http://localhost:33051"))
        {
            this.SendingRequest += Container_SendingRequest;
        }

        void Container_SendingRequest(object sender, System.Data.Services.Client.SendingRequestEventArgs e)
        {
            Console.WriteLine("{0} {1}", e.Request.Method, e.Request.RequestUri);
        }
    }
}
