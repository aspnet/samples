using Account.Client;
using System.Threading.Tasks;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public static class AccountClientExtensions
    {
        public static Task<HttpResult<ExternalLogin[]>> GetExternalLoginsAsync(this AccountClient accountClient)
        {
            return accountClient.GetExternalLoginsAsync(ClientFactory.BaseAddress);
        }

        public static Task<HttpResult<ManageInfo>> GetManageInfoAsync(this AccountClient accountClient)
        {
            return accountClient.GetManageInfoAsync(ClientFactory.BaseAddress);
        }
    }
}
