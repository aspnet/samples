using Account.Client;
using System.Threading.Tasks;
using WebApi.Client;
using Windows.Security.Credentials;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Todo.WindowsStore
{
    public sealed class AccessTokenProvider : IAccessTokenProvider
    {
        AppSettings settings = new AppSettings();

        Frame Frame
        {
            get
            {
                return (Frame)Window.Current.Content;
            }
        }

        public async Task<string> GetTokenAsync()
        {
            string accessToken = await PasswordVaultLoginAsync();
            if (accessToken != null) return accessToken;

            TaskCompletionSource<string> accessTokenSource = new TaskCompletionSource<string>();
            Frame.Navigate(typeof(LoginPage), accessTokenSource);
            return await accessTokenSource.Task;
        }

        public async Task<string> PasswordVaultLoginAsync()
        {
            PasswordCredential passwordCredential = settings.GetPasswordCredential();
            if (passwordCredential != null)
            {
                HttpResult<AccessTokenResponse> result;
                using (AccountClient accountClient = ClientFactory.CreateAccountClient())
                {
                    result = await accountClient.LoginAsync(passwordCredential.UserName, passwordCredential.Password);
                }
                if (result.Succeeded)
                {
                    return result.Content.AccessToken;
                }
                settings.ClearPasswordCredentials();
            }
            return null;
        }
    }
}
