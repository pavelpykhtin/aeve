using System.Threading.Tasks;
using Aeve.Contracts;

namespace Elbum.Application
{
    public interface IAeveClient
    {
        Task CreateMovieAsync(CreateMovieRequest request);
    }
}