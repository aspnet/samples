using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web;

namespace Todo.WindowsStore
{
    public static class ExternalLoginManager
    {   
        public static async Task<ExternalLoginResult> GetExternalAccessTokenAsync(string externalLoginUri, bool silentMode = false)
        {
            Uri authorizationRequestUri = new Uri(new Uri(ClientFactory.BaseAddress), externalLoginUri);
            WebAuthenticationOptions webAuthenticationOptions = silentMode ? WebAuthenticationOptions.SilentMode : WebAuthenticationOptions.None;
            WebAuthenticationResult authenticationResult = await WebAuthenticationBroker.AuthenticateAsync(webAuthenticationOptions, authorizationRequestUri);
            ExternalLoginResult loginExternalResult = new ExternalLoginResult() { WebAuthenticationResult = authenticationResult };
            if (authenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                Uri responseDataUri = new Uri(authenticationResult.ResponseData);
                string fragment = responseDataUri.Fragment;
                if (fragment != null && fragment.Length > 0)
                {
                    WwwFormUrlDecoder wwwFormUrlDecoder = new WwwFormUrlDecoder(fragment.Substring(1));
                    loginExternalResult.AccessToken = wwwFormUrlDecoder.GetFirstValueByName("access_token");
                }
            }
            return loginExternalResult;
        }
    }
}
