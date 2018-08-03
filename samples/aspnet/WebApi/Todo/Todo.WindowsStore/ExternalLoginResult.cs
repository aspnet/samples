using Windows.Security.Authentication.Web;

namespace Todo.WindowsStore
{
    public class ExternalLoginResult
    {
        public WebAuthenticationResult WebAuthenticationResult { get; set; }
        public string AccessToken { get; set; }
    }
}
