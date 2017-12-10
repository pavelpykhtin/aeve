using System.Collections.Generic;
using System.Threading.Tasks;
using Aeve.Application.Dal;
using Aeve.Application.Domain;
using Aeve.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Aeve.Application.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly IMapper mapper;

        public MoviesController(IMapper mapper, IMovieRepository movieRepository)
        {
            this.mapper = mapper;
            this.movieRepository = movieRepository;
        }

        [HttpGet]
        public async Task<List<MovieDto>> Get([FromQuery]int? pageIndex = 0, [FromQuery]int pageSize = 50)
        {
            var movies = await movieRepository.GetAsync(pageIndex ?? 0, pageSize);

            return mapper.Map<List<MovieDto>>(movies);
        }

        [HttpPost]
        public async Task Post([FromBody]CreateMovieRequest request)
        {
            var movie = mapper.Map<Movie>(request);

            await movieRepository.AddAsync(movie);
        }

        [HttpPost("postponed")]
        public async Task AddToPostponed([FromBody]PostponeMovieRequest request)
        {
            var movie = await movieRepository.GetAsync(request.Id);

            movie.IsPostponed = true;

            await movieRepository.UpdateAsync(movie);
        }

        [HttpGet("postponed")]
        public async Task<List<MovieDto>> GetPostponed([FromQuery]int? pageIndex = 0, [FromQuery]int pageSize = 50)
        {
            var movies = await movieRepository.GetPostponedAsync(pageIndex ?? 0, pageSize);

            return mapper.Map<List<MovieDto>>(movies);
        }

        [HttpPost("watched")]
        public async Task AddToWatched([FromBody]WatchMovieRequest request)
        {
            var movie = await movieRepository.GetAsync(request.Id);

            movie.IsWatched = true;

            await movieRepository.UpdateAsync(movie);
        }

        [HttpGet("watched")]
        public async Task<List<MovieDto>> GetWatched([FromQuery]int? pageIndex = 0, [FromQuery]int pageSize = 50)
        {
            var movies = await movieRepository.GetWatchedAsync(pageIndex ?? 0, pageSize);

            return mapper.Map<List<MovieDto>>(movies);
        }
    }
}
