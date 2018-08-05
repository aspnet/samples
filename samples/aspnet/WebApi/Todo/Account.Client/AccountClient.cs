using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Client;

namespace Account.Client
{
    public sealed class AccountClient : IDisposable
    {
        private const string Prefix = "api/account/";
        private const string TokenUri = "token";
        private const string ExternalLoginsUri = Prefix + "externallogins";
        private const string ExternalLoginUri = Prefix + "externallogin";
        private const string UserInfoUri = Prefix + "userinfo";
        private const string ChangePasswordUri = Prefix + "changepassword";
        private const string SetPasswordUri = Prefix + "setpassword";
        private const string AddExternalLoginUri = Prefix + "addexternallogin";
        private const string ManageInfoUri = Prefix + "manageinfo";
        private const string RegisterExternalUri = Prefix + "registerexternal";
        private const string RegisterUri = Prefix + "register";
        private const string RemoveLoginUri = Prefix + "removelogin";
        private const string LogoutUri = Prefix + "logout";

        private bool disposed;

        public AccountClient(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; set; }

        private async Task<HttpResult<AccessTokenResponse>> GetTokenAsync(IEnumerable<KeyValuePair<string, string>> values)
        {
            try
            {
                using (HttpResponseMessage response = await HttpClient.PostAsync(TokenUri, new FormUrlEncodedContent(values)))
                {
                    HttpResult<AccessTokenResponse> result = new HttpResult<AccessTokenResponse>() { StatusCode = response.StatusCode };
                    if (response.Content != null)
                    {
                        result.Content = await response.Content.ReadAsAsync<AccessTokenResponse>();
                    }
                    AccessTokenResponse tokenResponse = result.Content;
                    if (tokenResponse != null && tokenResponse.Error != null)
                    {
                        result.Errors.Add(tokenResponse.ErrorDescription);
                    }
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<AccessTokenResponse>.Failure(ex.Message);
            }
        }

        public Task<HttpResult<AccessTokenResponse>> LoginAsync(string userName, string password)
        {
            ThrowIfDisposed();
            return GetTokenAsync(new Dictionary<string, string>()
            {
                { "grant_type", "password" },
                { "username", userName },
                { "password", password }
            });
        }

        public async Task<HttpResult<ExternalLogin[]>> GetExternalLoginsAsync(string returnUrl)
        {
            ThrowIfDisposed();
            return await HttpClient.GetAsync<ExternalLogin[]>(ExternalLoginsUri + "?returnUrl=" + Uri.EscapeDataString(returnUrl));
        }

        public async Task<HttpResult<UserInfo>> GetUserInfoAsync()
        {
            ThrowIfDisposed();
            return await HttpClient.GetAsync<UserInfo>(UserInfoUri);
        }

        public async Task<HttpResult<RegisterResponse>> RegisterAsync(RegisterUser registerUser)
        {
            ThrowIfDisposed();
            HttpResult<RegisterResponse> result = await HttpClient.PostAsJsonAsync<RegisterUser, RegisterResponse>(RegisterUri, registerUser);
            AddAllErrors(result, result.Content);
            return result;
        }

        public async Task<HttpResult<RegisterExternalResponse>> RegisterExternalAsync(RegisterExternalUser externalUser)
        {
            ThrowIfDisposed();
            HttpResult<RegisterExternalResponse> result = await HttpClient.PostAsJsonAsync<RegisterExternalUser, RegisterExternalResponse>(RegisterExternalUri, externalUser);
            AddAllErrors(result, result.Content);
            return result;
        }

        public Task<HttpResult<ManageInfo>> GetManageInfoAsync(string returnUrl)
        {
            ThrowIfDisposed();
            return HttpClient.GetAsync<ManageInfo>(ManageInfoUri + "?returnUrl=" + Uri.EscapeDataString(returnUrl));
        }

        public async Task<HttpResult<ErrorResponse>> RemoveLoginAsync(RemoveLogin removeLogin)
        {
            ThrowIfDisposed();
            HttpResult<ErrorResponse> result = await HttpClient.PostAsJsonAsync<RemoveLogin, ErrorResponse>(RemoveLoginUri, removeLogin);
            AddAllErrors(result, result.Content);
            return result;
        }

        public async Task<HttpResult<ErrorResponse>> AddExternalLoginAsync(AddExternalLogin addExternalLogin)
        {
            ThrowIfDisposed();
            HttpResult<ErrorResponse> result = await HttpClient.PostAsJsonAsync<AddExternalLogin, ErrorResponse>(AddExternalLoginUri, addExternalLogin);
            AddAllErrors(result, result.Content);
            return result;
        }

        public async Task<HttpResult<ChangePasswordResponse>> ChangePasswordAsync(ChangePassword changePassword)
        {
            ThrowIfDisposed();
            HttpResult<ChangePasswordResponse> result = await HttpClient.PostAsJsonAsync<ChangePassword, ChangePasswordResponse>(ChangePasswordUri, changePassword);
            AddAllErrors(result, result.Content);
            return result;
        }

        public async Task<HttpResult<SetPasswordResponse>> SetPasswordAsync(SetPassword setPassword)
        {
            ThrowIfDisposed();
            HttpResult<SetPasswordResponse> result = await HttpClient.PostAsJsonAsync<SetPassword, SetPasswordResponse>(SetPasswordUri, setPassword);
            AddAllErrors(result, result.Content);
            return result;
        }

        private static void AddAllErrors(HttpResult result, ErrorResponse errorResponse)
        {
            if (errorResponse != null)
            {
                foreach (string error in errorResponse.AllErrors)
                {
                    result.Errors.Add(error);
                }
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                HttpClient.Dispose();
                disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }
    }
}
