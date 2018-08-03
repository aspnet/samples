using System;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using WebApi.Client;
using Windows.Security.Authentication.Web;

namespace Todo.WindowsPhone
{
    public static class ExternalLoginManager
    {
        public static async Task<ExternalLoginResult> GetExternalAccessTokenAsync(string externalLoginUri)
        {
            Uri authorizationRequestUri = new Uri(new Uri(ClientFactory.BaseAddress), externalLoginUri);
           
            Uri endUri = new Uri(authorizationRequestUri, "/");
            WebAuthenticationResult authenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None, 
                authorizationRequestUri, 
                endUri);
            ExternalLoginResult loginExternalResult = new ExternalLoginResult() { WebAuthenticationResult = authenticationResult };
            if (authenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                Uri responseDataUri = new Uri(authenticationResult.ResponseData);
                loginExternalResult.AccessToken = GetAccessTokenFromFragment(responseDataUri);
            }
            return loginExternalResult;
        }

        static string GetAccessTokenFromFragment(Uri uri)
        {
            HttpValueCollection httpValues = uri.ParseFragment();
            string[] accessTokens = httpValues.GetValues("access_token");
            return accessTokens.Length > 0 ? accessTokens[0] : null;
        }
    }
}
