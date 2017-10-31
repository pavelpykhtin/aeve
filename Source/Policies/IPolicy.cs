using System;
using System.Threading.Tasks;

namespace Policies
{
    public interface IPolicy
    {
        Task ApplyAsync(Func<Task> action);
        Task<TResponse> ApplyAsync<TResponse>(Func<Task<TResponse>> action);
    }
}