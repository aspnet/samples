using Account.Client;
using System;
using System.Net.Http;
using Todo.Client;

namespace Todo.WindowsPhone
{
    public static class ClientFactory
    {
        public const string BaseAddress = "https://securetodo.azurewebsites.net/";

        public static HttpClient CreateHttpClient()
        {
            AppSettings settings = new AppSettings();
            AccessTokenProvider loginProvider = new AccessTokenProvider();
            OAuth2BearerTokenHandler oauth2Handler = new OAuth2BearerTokenHandler(settings, loginProvider);
            HttpClient httpClient = HttpClientFactory.Create(oauth2Handler);
            httpClient.BaseAddress = new Uri(BaseAddress);
            httpClient.Timeout = TimeSpan.FromDays(1);
            return httpClient;
        }  

        public static AccountClient CreateAccountClient()
        {
            return new AccountClient(CreateHttpClient());
        }

        public static TodoClient CreateTodoClient()
        {
            return new TodoClient(CreateHttpClient());
        }
    }
}
