using System.Web.Http;
using System.Web.Routing;

namespace HostedClientCertificateSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        // This sample shows how to secure a web API server with mutual Certificates in the web hosted scenario. It also shows how to authenticate and authorize based
        // on client's certificate. This project contains the server part.

        // There are some server related SETUP steps required to run this app successfully. Please go the ../Client/Program.cs to find the client related instructions.

        // STEP 1: Modify the host file under C:\Windows\System32\drivers\etc to add the following mapping from contoso.com to localhost
        // 127.0.0.1       contoso.com

        // STEP 2: Generate your own server test cert using the following makecert command. Please know that you must run the following command as administrator.
        //      makecert -pe -n "CN=contoso.com" -ss my -sr LocalMachine -a sha1 -sky signature -r
        //   If makecert is not in your path, try locations such as 'C:\Program Files (x86)\Windows Kits\8.1\bin\x64'

        // STEP 3: Add the https mapping in the Default Web Site's Bindings section, and use the contoso.com certificate for server certificate;

        // STEP 4: Go to the IIS manager, SSL setting, check require SSL and select "Allow" (or "Accept" for some versions) client certificate
        // so that client certificate is not required for anonymous access for the GET actions.

        // Step 5: Update the ClientCertHash in CustomCertificateMessageHandler file with your client certificate's hash.
        //
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

            // Register a message to turn the client cert to a admin role
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CustomCertificateMessageHandler());
        }
    }
}
