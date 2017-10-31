using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Policies;
using Serializer;

namespace JsonHttpClient
{
    public class JsonHttpClient
    {
        private readonly HttpClient client;
        private readonly ISerializer serializer;
        private readonly IPolicy policy;

        public JsonHttpClient(string baseUrl, ISerializer serializer, IPolicy policy)
            : this(baseUrl, serializer, null, policy)
        {

        }

        public JsonHttpClient(string baseUrl, ISerializer serializer, WebProxy proxy, IPolicy policy)
        {
            this.serializer = serializer;
            this.policy = policy;
            var messageHandler = new HttpClientHandler();
            if (proxy != null)
            {
                messageHandler.Proxy = proxy;
                messageHandler.UseProxy = true;
            }

            client = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public Task PutOneWayAsync(string url, object request)
        {
            return policy.ApplyAsync(() => RequestOneWayCoreAsync(client.PutAsync, url, request));
        }

        public Task<TResponse> PutAsync<TResponse>(string url, object request)
        {
            return policy.ApplyAsync(() => RequestCoreAsync<TResponse>(client.PutAsync, url, request));
        }

        public Task<TResponse> GetAsync<TResponse>(string url)
        {
            return policy.ApplyAsync(() => RequestCoreAsync<TResponse>(uri => client.GetAsync(uri, new CancellationTokenSource().Token), url));
        }

        public Task DeleteOneWayAsync(string url)
        {
            return policy.ApplyAsync(() => RequestOneWayCoreAsync(uri => client.DeleteAsync(uri, new CancellationTokenSource().Token), url));
        }

        public Task<TResponse> DeleteAsync<TResponse>(string url)
        {
            return policy.ApplyAsync(() => RequestCoreAsync<TResponse>(client.DeleteAsync, url));
        }

        public Task PostOneWayAsync(string url, object request)
        {
            return policy.ApplyAsync(() => RequestOneWayCoreAsync(client.PostAsync, url, request));
        }

        public Task<TResponse> PostAsync<TResponse>(string url, object request)
        {
            return policy.ApplyAsync(() => RequestCoreAsync<TResponse>(client.PostAsync, url, request));
        }

        private async Task<TResponse> RequestCoreAsync<TResponse>(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, object request)
        {
            HttpContent content = new StringContent(serializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await method(url, content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return serializer.Deserialize<TResponse>(responseContent);
        }

        private async Task<TResponse> RequestCoreAsync<TResponse>(Func<string, Task<HttpResponseMessage>> method, string url)
        {
            var response = await method(url);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return serializer.Deserialize<TResponse>(responseContent);
        }

        private async Task RequestOneWayCoreAsync(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, object request)
        {
            HttpContent content = new StringContent(serializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await method(url, content);

            response.EnsureSuccessStatusCode();
        }

        private async Task RequestOneWayCoreAsync(Func<string, Task<HttpResponseMessage>> method, string url)
        {
            var response = await method(url);

            response.EnsureSuccessStatusCode();
        }
    }
}