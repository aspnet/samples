using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Account.Client
{
    public class OAuth2BearerTokenHandler : HttpAuthenticationHandler
    {
        const string BearerScheme = "Bearer";

        public OAuth2BearerTokenHandler(IAccessTokenStore tokenStore, IAccessTokenProvider tokenProvider)
        {
            if (tokenStore == null)
            {
                throw new ArgumentNullException("tokenStore");
            }
            if (tokenProvider == null)
            {
                throw new ArgumentNullException("tokenProvider");
            }

            this.TokenStore = tokenStore;
            this.TokenProvider = tokenProvider;
        }

        public IAccessTokenStore TokenStore { get; private set; }
        public IAccessTokenProvider TokenProvider { get; set; }

        protected override async Task<AuthenticationHeaderValue> CreateAuthorizationHeaderAsync(HttpRequestMessage request, HttpHeaderValueCollection<AuthenticationHeaderValue> challenges, CancellationToken cancellationToken)
        {
            AuthenticationHeaderValue bearerChallenge = challenges.FirstOrDefault(IsOAuth2BearerChallenge);
            if (bearerChallenge != null)
            {
                string accessToken = await GetAccessTokenAsync(request);
                if (accessToken != null)
                {
                    return new AuthenticationHeaderValue(BearerScheme, accessToken);                 
                }
            }
 	        return null;
        }

        bool IsOAuth2BearerChallenge(AuthenticationHeaderValue authenticationHeader)
        {
            return authenticationHeader.Scheme.Equals(BearerScheme, StringComparison.OrdinalIgnoreCase);
        }

        protected virtual async Task<string> GetAccessTokenAsync(HttpRequestMessage request)
        {
            string accessToken = TokenStore.AccessToken;
            if (accessToken != null)
            {
                if (!request.HasAccessToken(accessToken))
                {
                    // The request tried a different access token than the one the store has.
                    // Try again with the cached access token from the store.
                    return accessToken;
                }
                // We just tried this access token but it didn't work; get rid of it.
                TokenStore.AccessToken = null;
            }

            TokenStore.AccessToken = await TokenProvider.GetTokenAsync();
            return TokenStore.AccessToken;
        }
    }
}
