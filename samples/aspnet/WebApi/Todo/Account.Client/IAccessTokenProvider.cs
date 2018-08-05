using System.Threading.Tasks;

namespace Account.Client
{
    public interface IAccessTokenProvider
    {
        Task<string> GetTokenAsync();
    }
}
