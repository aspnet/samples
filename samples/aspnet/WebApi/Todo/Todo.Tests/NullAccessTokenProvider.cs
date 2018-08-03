using Account.Client;
using System.Threading.Tasks;

namespace Todo.Tests
{
    class NullAccessTokenProvider : IAccessTokenProvider
    {
        public Task<string> GetTokenAsync()
        {
            return Task.FromResult<string>(null);
        }
    }
}
