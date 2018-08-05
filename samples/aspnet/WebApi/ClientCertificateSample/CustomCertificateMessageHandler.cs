using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace ClientCertificateSample
{
    /// <summary>
    /// This verifies the client certificate is one of those known certificates and creates an generic principals with "Administrators" role. 
    /// </summary>
    public class CustomCertificateMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Retrieving the Client certificate sent by the client
            X509Certificate cert = request.GetClientCertificate();

            // The following code SHOULD be replaced with some custom mapping logic from client certificate to their roles. 
            // The client certificate is not being checked for certificate revocation or ensure PKI validity. 
            if (cert != null)
            {
                if (cert.GetCertHashString() == Program.ClientCertHash)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(cert.Subject), new[] { "Administrators" });
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
