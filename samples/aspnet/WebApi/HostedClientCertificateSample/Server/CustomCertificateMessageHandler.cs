using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HostedClientCertificateSample
{
    /// <summary>
    /// This verifies the client certificate is one of those known certificates and creates a generic principal with "Administrators" role.
    /// </summary>
    public class CustomCertificateMessageHandler : DelegatingHandler
    {
        public const string ClientCertHash = "97526DCB0CC8D557436DBA57EE1116CFB81E80E1";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Retrieving the Client certificate sent by the client
            X509Certificate cert = request.GetClientCertificate();

            // The following code SHOULD be replaced with some custom mapping logic from client certificate to their roles.
            if (cert != null)
            {
                if (cert.GetCertHashString() == ClientCertHash)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(cert.Subject, "X509Certificate"), new[] { "Administrators" });

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = Thread.CurrentPrincipal;
                    }
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
