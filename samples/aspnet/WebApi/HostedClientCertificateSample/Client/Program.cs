using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HostedClientCertificateSample
{
    /// <summary>
    /// This sample shows how to secure a web API server with mutual Certificates in the web hosted scenario. It also shows how to authenticate and authorize based
    /// on client's certificate. This project contains the client part.
    ///
    /// There are some Client related SETUP steps required to run this app successfully. For the server related steps, please see those instructions in Global.asax.cs.
    ///
    /// Step 1: Generate your client test cert using the following makecert command.
    ///      makecert -pe -n "CN=Your Name" -ss my -sr CurrentUser -a sha1 -sky signature -r
    ///   If makecert is not in your path, try locations such as 'C:\Program Files (x86)\Windows Kits\8.1\bin\x64'
    ///
    /// Step 2: Export the client test certificate from the CurrentUser/Personal certificate store, and then import newly exported certificate into the Local Machine store under
    /// "Trusted Root Certification Authority" so that the server will trust this client certificate. After you complete the testing, please uninstall the certificate from the
    /// "Trusted Root Certification Authority" store.
    ///
    /// Step 3: Update the ServerCertHash with hash of your server certificate and the ClientCertHash with hash of your client certificate.
    /// Please also update the GetClientCertificate() method to return the client certificate if it needs to come from a different store other than CurrentUser/Personal.
    ///
    /// </summary>
    class Program
    {
        static readonly Uri _baseAddress = new Uri("https://contoso.com/HostedClientCertificateSample/");

        // Update here with hash of your client and server certificates
        public const string ServerCertHash = "3DD9A794612C78E25B1FD25ECC4E486C18E690D2";
        public const string ClientCertHash = "97526DCB0CC8D557436DBA57EE1116CFB81E80E1";

        static void Main(string[] args)
        {
            // RunClient
            RunClient();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }

        private static void RunClient()
        {
            // TEST CERTIFICATE ONLY, PLEASE REMOVE WHEN YOU REPLACE THE CERT WITH A REAL CERT
            // Perform the Server Certificate validation
            ServicePointManager.ServerCertificateValidationCallback += Program.RemoteCertificateValidationCallback;

            // set up the client without client certificate
            HttpClient client = new HttpClient();
            client.BaseAddress = _baseAddress;
            
            // set up the client with client certificate
            WebRequestHandler handler = new WebRequestHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual; // this would pick from the Current user store
            handler.ClientCertificates.Add(GetClientCertificate());

            HttpClient clientWithClientCert = new HttpClient(handler);
            clientWithClientCert.BaseAddress = _baseAddress;
            HttpResponseMessage response;

            //// How to post a sample item with success
            SampleItem sampleItem = new SampleItem { Id = 1, StringValue = "hello from a new sample item" };
            response = clientWithClientCert.PostAsJsonAsync("api/sample/", sampleItem).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Posting the first item returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to get all the sample Items, we should see the newly added sample item that we just posted
            response = client.GetAsync("api/sample/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Getting all the items returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to post another sample item with success
            sampleItem = new SampleItem { Id = 2, StringValue = "hello from a second new sample item" };
            response = clientWithClientCert.PostAsJsonAsync("api/sample/", sampleItem).Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Posting a second item returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to get all the sample Items, we should see the two newly added sample item that we just posted
            response = client.GetAsync("api/sample/").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Getting all the items returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // How to get an sample item with id = 2
            response = client.GetAsync("api/sample/2").Result;
            response.EnsureSuccessStatusCode();
            Console.WriteLine("Getting the second item returns:\n" + response.Content.ReadAsStringAsync().Result + "\n");
            response.Dispose();

            // Posting without client certificate will fail 
            sampleItem = new SampleItem { Id = 3, StringValue = "hello from a new sample item" };
            response = client.PostAsJsonAsync("api/sample/", sampleItem).Result;
            Console.WriteLine("Posting the third item without client certificate fails:\n" + response.StatusCode + "\n");
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
    }

    public class SampleItem
    {
        public int Id { get; set; }

        public string StringValue { get; set; }

        public override string ToString()
        {
            return Id + "," + StringValue;
        }
    }
}
