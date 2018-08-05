using System;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace BasicAuthentication.Filters
{
    public abstract class BasicAuthenticationAttribute : FilterAttribute, IAuthenticationFilter
    {
        public string Realm { get; set; }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            string[] values = request.Headers.GetValues("Authorization");

            if (values == null || values.Length != 1)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or Result (which would indicate an error).
                return;
            }

            string value = values[0];

            if (value == null || !value.StartsWith("Basic "))
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or Result (which would indicate an error).
                return;
            }

            string encodedCredentials = value.Substring("Basic ".Length);

            if (encodedCredentials != null)
            {
                encodedCredentials = encodedCredentials.TrimStart(' ');
            }

            if (String.IsNullOrEmpty(encodedCredentials))
            {
                // Authentication was attempted but failed. Set Result to indicate an error.
                filterContext.Result = new HttpStatusCodeResult(401, "Missing credentials");
                return;
            }

            Tuple<string, string> userNameAndPasword = ExtractUserNameAndPassword(encodedCredentials);

            if (userNameAndPasword == null)
            {
                // Authentication was attempted but failed. Set Result to indicate an error.
                filterContext.Result = new HttpStatusCodeResult(401, "Invalid credentials");
                return;
            }

            string userName = userNameAndPasword.Item1;
            string password = userNameAndPasword.Item2;

            IPrincipal principal = Authenticate(userName, password);

            if (principal == null)
            {
                // Authentication was attempted but failed. Set Result to indicate an error.
                filterContext.Result = new HttpStatusCodeResult(401, "Invalid username or password");
            }
            else
            {
                // Authentication was attempted and succeeded. Set Principal to the authenticated user.
                filterContext.Principal = principal;
            }
        }

        protected abstract IPrincipal Authenticate(string userName, string password);

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            string challenge;

            if (String.IsNullOrEmpty(Realm))
            {
                challenge = "Basic";
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                challenge = "Basic realm=\"" + Realm + "\"";
            }

            filterContext.ChallengeWith(challenge);
        }

        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            string userName = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }
    }
}
