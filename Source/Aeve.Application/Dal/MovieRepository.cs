using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Aeve.Application.Dal
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> collection;
        private readonly IMapper mapper;

        public MovieRepository(IMapper mapper, string connectionString)
        {
            this.mapper = mapper;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("Aeve");

            collection = database.GetCollection<Movie>(nameof(Movie), new MongoCollectionSettings
            {
                AssignIdOnInsert = true
            });
        }

        public async Task<List<Domain.Movie>> GetAsync(int pageIndex, int pageSize)
        {
            var movies = await collection.AsQueryable()
                .Where(x => (float) x.Rating > 6.5)
                .Where(x => x.IsWatched == false)
                .OrderByDescending(x => x.PublishedAt)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return mapper.Map<List<Domain.Movie>>(movies);
        }

        public async Task<Domain.Movie> GetAsync(Guid id)
        {
            var movie = await collection.AsQueryable()
                .Where(x => x.Id == id)
                .SingleAsync();

            return mapper.Map<Domain.Movie>(movie);
        }

        public async Task AddAsync(Domain.Movie movie)
        {
            var movieImpl = mapper.Map<Movie>(movie);

            await collection.InsertOneAsync(movieImpl);
        }

        public async Task UpdateAsync(Domain.Movie movie)
        {
            var internalMovie = mapper.Map<Movie>(movie);

            await collection.ReplaceOneAsync(x => x.Id == movie.Id, internalMovie, new UpdateOptions {IsUpsert = true});
        }

        public async Task<List<Domain.Movie>> GetPostponedAsync(int pageIndex, int pageSize)
        {
            var movies = await collection.AsQueryable()
                .Where(x => x.IsPostponed)
                .OrderByDescending(x => x.PublishedAt)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return mapper.Map<List<Domain.Movie>>(movies);
        }

        public async Task<List<Domain.Movie>> GetWatchedAsync(int pageIndex, int pageSize)
        {
            var movies = await collection.AsQueryable()
                .Where(x => x.IsWatched == true)
                .OrderByDescending(x => x.PublishedAt)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return mapper.Map<List<Domain.Movie>>(movies);
        }

        public class Movie
        {
            public Guid Id { get; set; }
            public string ExternalId { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string Description { get; set; }
            public double? Rating { get; set; }
            public int? Year { get; set; }
            public long PublishedAt { get; set; }
            public string Url { get; set; }
            public bool IsPostponed { get; set; }
            public bool IsWatched { get; set; }
        }
    }
}