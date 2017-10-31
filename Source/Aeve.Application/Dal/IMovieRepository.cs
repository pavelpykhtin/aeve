using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aeve.Application.Domain;

namespace Aeve.Application.Dal
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAsync(int pageIndex, int pageSize);
        Task<Movie> GetAsync(Guid id);
        Task AddAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task<List<Movie>> GetPostponedAsync(int pageIndex, int pageSize);
        Task<List<Movie>> GetWatchedAsync(int pageIndex, int pageSize);
    }
}