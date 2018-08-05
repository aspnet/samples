using Account.Client;
using System.Threading.Tasks;
using WebApi.Client;
using Windows.Security.Authentication.Web;

namespace Todo.WindowsStore
{
    public static class AccountClientExtensions
    {
        static string returnUrl = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;

        public static Task<HttpResult<ExternalLogin[]>> GetExternalLoginsAsync(this AccountClient accountClient)
        {
            return accountClient.GetExternalLoginsAsync(returnUrl);
        }

        public static Task<HttpResult<ManageInfo>> GetManageInfoAsync(this AccountClient accountClient)
        {
            return accountClient.GetManageInfoAsync(returnUrl);
        }
    }
}
