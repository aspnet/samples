using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Account.Client
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetAccessToken(this HttpRequestMessage request)
        {
            AuthenticationHeaderValue authorizationHeader = request.Headers.Authorization;
            if (authorizationHeader != null)
            {
                if (authorizationHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    return authorizationHeader.Parameter;
                }
            }
            return null;
        }

        public static bool HasAccessToken(this HttpRequestMessage request, string accessToken)
        {
            return accessToken != null && request.GetAccessToken() == accessToken;
        }
    }
}
