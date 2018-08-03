using Account.Client;

namespace Todo.Tests
{
    public class InMemoryAccessTokenStore : IAccessTokenStore
    {
        public string AccessToken { get; set; }

        public void ClearAccessToken()
        {
            AccessToken = null;
        }

        public string GetAccessToken()
        {
            return AccessToken;
        }

        public void SaveAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
