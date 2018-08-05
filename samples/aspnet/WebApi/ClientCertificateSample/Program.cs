using System;
using System.IdentityModel.Selectors;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Web.Http;
using System.Web.Http.SelfHost;
using ClientCertificateSample.Models;

namespace ClientCertificateSample
{
    /// <summary>
    /// This sample shows how to secure a web API server with mutual Certificates in the self host scenario. It also shows how to authenticate and authorize based 
    /// on client's certificate. 
    ///
    /// There are some SETUP steps required to run this app successfully.
    /// 
    /// STEP 1: Generate your own server test cert using the following makecert cmd. Please note that you must run the following command as administrator.
    ///      makecert -pe -n "CN=contoso.com" -ss my -sr LocalMachine -a sha1 -sky signature -r

    /// STEP 2: Map the SSL cert to the selfhost port with the cert hash for your server certificate you just generated. e.g. 
    ///     netsh http add sslcert ipport=0.0.0.0:50231 certhash=F8A657C399C919DC96F0C272D35CE5D51880CEF6 appid={DAEFA3B4-8827-47B3-9981-004E63F5DA59}
    ///     

    /// STEP 3: Generate your own client test cert using the following makecert cmd. 
    ///      makecert -pe -n "CN=Your Name" -ss my -sr CurrentUser -a sha1 -sky signature -r

    /// STEP 4: Export the client test certificate from the CurrentUser/Personal certificate store, and then import newly exported certificate into the Local Machine store under 
    /// "Trusted Root Certification Authority" so that the server will trust this client certificate. After you complete the testing, please uninstall the certificate from the  
    /// "Trusted Root Certification Authority" store.

    /// Please update the ServerCertHash with the hash value of your server certificate and the ClientCertHash with the hash of your client certificate.
    /// If you need to select a client certificate from a different store other than CurrentUser/Personal, please update the GetClientCertificate() method to return 
    /// the client certificate your app uses.
    
    /// CLEAN UP STEPS
    ///    netsh http delete sslcert ipport=0.0.0.0:50231
    ///    uninstall the certificate from the LocalMachine/"Trusted Root Certification Authority" store.
    ///    
    /// </summary>
    class Program
    {
        static readonly Uri _baseAddress = new Uri("https://localhost:50231/");

        // Update here with your client and server certificate 
        public const string ServerCertHash = "F8A657C399C919DC96F0C272D35CE5D51880CEF6";
        public const string ClientCertHash = "496B1B29D45375D49270966A45811E9F0AC870E2";

        static void Main(string[] args)
        {
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // tell the system that the server requires client cert
            config.ClientCredentialType = HttpClientCredentialType.Certificate;

            // turn off the built in certificate authentication
            config.X509CertificateValidator = new NoOpValidator();

            // set up the message handler to convert the client certificate to principal with Administrator role
            config.MessageHandlers.Add(new CustomCertificateMessageHandler());

            HttpSelfHostServer server = null;
            try
            {
                // create the server 
                server = new HttpSelfHostServer(config);

                // Start listening
                server.OpenAsync().Wait();
                Console.WriteLine("Listening on " + _baseAddress + "...\n");

                // RunClient
                RunClient();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not start server: {0}", e.GetBaseException().Message);
            }
            finally
            {
                Console.WriteLine("Hit ENTER to exit...");
                Console.ReadLine();

                if (server != null)
                {
                    // Stop listening
                    server.CloseAsync().Wait();
                }
            }
        }

        private static void RunClient()
        {
            // TEST CERTIFICATE ONLY, PLEASE REMOVE WHEN YOU REPLACE THE CERT WITH A REAL CERT
            // Perform the Server Certificate validation
            ServicePointManager.ServerCertificateValidationCallback += Program.RemoteCertificateValidationCallback;

            // start the client
            WebRequestHandler handler = new WebRequestHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual; // this would pick from the Current user store
            handler.ClientCertificates.Add(GetClientCertificate());
            
            HttpClient client = new HttpClient(handler);
            client.BaseAddress = _baseAddress;
            HttpResponseMessage response;

            // How to post a sample item with success
            SampleItem sampleItem = new SampleItem { Id = 1, StringValue = "hello from a new sample item" };
            response = client.PostAsJsonAsync("/api/sample/", sampleItem).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Posting the first item returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to get all the sample Items, we should see the newly added sample item that we just posted
            response = client.GetAsync("/api/sample/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Getting all the items returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to post another sample item with success
            sampleItem = new SampleItem { Id = 2, StringValue = "hello from a second new sample item" };
            response = client.PostAsJsonAsync("/api/sample/", sampleItem).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Posting a second item returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to get all the sample Items, we should see the two newly added sample item that we just posted
            response = client.GetAsync("/api/sample/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Getting all the items returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to get an sample item with id = 2
            response = client.GetAsync("/api/sample/2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Getting the second item returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();
        }

        // TEST CERTIFICATE ONLY, PLEASE REMOVE YOU WHEN REPLACE THE CERT WITH A REAL CERT
        public static bool RemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return certificate.GetCertHashString() == ServerCertHash;
        }

        // You SHOULD update here to if you client certificate locates in other x509 store
        private static X509Certificate GetClientCertificate()
        {
           X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509CertificateCollection col = store.Certificates.Find(X509FindType.FindByThumbprint, ClientCertHash, false);

            if (col.Count == 1)
            {
                return col[0];
            }
            else
            {
                throw new ApplicationException("Cannot find a certificate. Please follow the setup steps at the beginning of this file to import the Client certificate.");
            }
        }

        /// <summary>
        /// This class disables the default certificate validation, we are doing the client certificate checking in the CustomCertificateMessageHandler. 
        /// Please note that this is not performing revocation check any more. It is app's responsibility to perform proper validation in the CustomCertificateMessageHandler. 
        /// </summary>
        class NoOpValidator : X509CertificateValidator
        {
            public override void Validate(X509Certificate2 certificate)
            {
               return;
            }
        }
    }
}
