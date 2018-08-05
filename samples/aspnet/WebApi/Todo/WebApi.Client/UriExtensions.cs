using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace WebApi.Client
{
    public static class UriExtensions
    {
        public static HttpValueCollection ParseFragment(this Uri uri)
        {
            UriBuilder builder = new UriBuilder(uri);
            builder.Query = uri.Fragment.TrimStart('#');
            return builder.Uri.ParseQueryString();
        }
    }
}
