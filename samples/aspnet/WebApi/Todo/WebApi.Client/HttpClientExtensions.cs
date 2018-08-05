using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace WebApi.Client
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResult<T>> GetAsync<T>(this HttpClient client, string address)
        {
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(address))
                {
                    HttpResult<T> result = new HttpResult<T>() { StatusCode = response.StatusCode };
                    if (response.Content != null)
                    {
                        result.Content = await response.Content.ReadAsAsync<T>();
                    }
                    return result;
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<T>.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult<TResponse>> PostAsJsonAsync<TRequest, TResponse>(this HttpClient client, string address, TRequest content)
        {
            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, address))
                {
                    request.Content = new ObjectContent<TRequest>(content, GetJsonFormatter());
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        HttpResult<TResponse> result = new HttpResult<TResponse>() { StatusCode = response.StatusCode };
                        if (response.Content != null)
                        {
                            result.Content = await response.Content.ReadAsAsync<TResponse>();
                        }
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<TResponse>.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult> DeleteItemAsync(this HttpClient client, string address)
        {
            try
            {
                using (HttpResponseMessage response = await client.DeleteAsync(address))
                {
                    return new HttpResult() { StatusCode = response.StatusCode };
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult.Failure(ex.Message);
            }
        }

        public static async Task<HttpResult<T>> PatchAsync<T>(this HttpClient client, string address, T patch, bool returnContent = false)
        {
            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), address))
                {
                    request.Content = new ObjectContent<T>(patch, GetJsonFormatter());
                    request.Headers.Add("Prefer", returnContent ? "return-content" : "return-no-content");
                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        HttpResult<T> result = new HttpResult<T>() { StatusCode = response.StatusCode };
                        if (returnContent)
                        {
                            result.Content = await response.Content.ReadAsAsync<T>();
                        }
                        return result;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return HttpResult<T>.Failure(ex.Message);
            }
        }

        static MediaTypeFormatter GetJsonFormatter()
        {
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            return formatter;
        }
    }
}
