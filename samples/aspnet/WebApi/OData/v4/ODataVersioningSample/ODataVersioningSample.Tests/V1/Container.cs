using System;

namespace ODataVersioningSample.Tests.V1.Default
{
    public partial class Container
    {
        private const string BaseAddress = "http://localhost:50232";

        public static Container New()
        {
            // Switch to use different versioning method
            return CreateContainerVersioningByRoute();
            //return CreateContainerVersioningByQueryString();
            //return CreateContainerVersioningByHeader();
        }

        private static Container CreateContainerVersioningByRoute()
        {
            Container c = new Container(new Uri(BaseAddress + "/versionbyroute/v1"));
            return c;
        }

        private static Container CreateContainerVersioningByQueryString()
        {
            Container c = new Container(new Uri(BaseAddress + "/versionbyquery"));
            c.BuildingRequest += (sender, e) =>
            {
                UriBuilder builder = new UriBuilder(e.RequestUri);
                if (string.IsNullOrEmpty(builder.Query))
                {
                    builder.Query = "v=1";
                }
                else
                {
                    builder.Query += "&v=1";
                }

                e.RequestUri = builder.Uri;
            };
            return c;
        }

        private static Container CreateContainerVersioningByHeader()
        {
            Container c = new Container(new Uri(BaseAddress + "/versionbyheader"));
            c.SendingRequest2 += (sender, e) =>
            {
                e.RequestMessage.SetHeader("v", "1");
            };
            return c;
        }
    }
}
