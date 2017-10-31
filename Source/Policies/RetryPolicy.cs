using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace Policies
{
    public class RetryPolicy: IPolicy
    {
        public Task ApplyAsync(Func<Task> action)
        {
            return Policy
                .Handle<HttpRequestException>()
                .RetryAsync(3)
                .ExecuteAsync(action);
        }

        public Task<TResponse> ApplyAsync<TResponse>(Func<Task<TResponse>> action)
        {
            return Policy<TResponse>
                .Handle<HttpRequestException>()
                .RetryAsync(3)
                .ExecuteAsync(action);
        }
    }
}