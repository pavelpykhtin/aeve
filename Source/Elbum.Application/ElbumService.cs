using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Aeve.Contracts;
using Elbum.Application.Contracts;
using Polly;
using Serializer;
using MovieDto = Elbum.Application.Contracts.MovieDto;

namespace Elbum.Application
{
    public class ElbumService
    {
        private string baseUrl = "https://zona.mobi";
        private readonly HttpClient client;
        private readonly ISerializer serializer;
        private readonly IAeveClient aeveClient;

        public ElbumService(ISerializer serializer, IAeveClient aeveClient)
        {
            this.serializer = serializer;
            this.aeveClient = aeveClient;
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public Task Run(CancellationToken token)
        {
            return CollectAsync(token);
        }

        private async Task CollectAsync(CancellationToken token)
        {
            try
            {
                var nextPage = "/movies/filter/sort-date";
                do
                {
                    var page = await Policy
                        .Handle<HttpRequestException>()
                        .Retry(3)
                        .Execute(() => FetchPage(nextPage));

                    var uploadTasks = page.Items.Select(HandleMovie).ToList();

                    await Task.WhenAll(uploadTasks);

                    nextPage = page.Pagination.NextUrl;

                } while (nextPage != null && !token.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task HandleMovie(MovieDto arg)
        {
            var details = await Policy
                .Handle<HttpRequestException>()
                .Retry(3)
                .Execute(() => FetchMovieDetails(arg.NameId));

            await PostMovie(arg, details);
        }

        private async Task PostMovie(MovieDto movieDto, MovieDetailsDto detailsDescription)
        {
            await aeveClient.CreateMovieAsync(new CreateMovieRequest
            {
                Id = Guid.NewGuid(),
                Description = detailsDescription.Description,
                ExternalId = movieDto.MobiLinkId,
                Image = movieDto.Cover,
                Name = movieDto.NameRus,
                Rating = (decimal?)movieDto.RatingKinopoisk,
                Year = movieDto.Year,
                PublishedAt = detailsDescription.MobiLinkDate
            });
        }

        private async Task<MoviePageDto> FetchPage(string pageUrl)
        {
            var pageResponse = await client.GetStringAsync(pageUrl);

            return serializer.Deserialize<MoviePageDto>(pageResponse);
        }

        private async Task<MovieDetailsDto> FetchMovieDetails(string movieNameId)
        {
            var detailsResponse = await client.GetStringAsync($"{baseUrl}/movies/{movieNameId}");

            return serializer.Deserialize<MovieDetailsConainerDto>(detailsResponse).Movie;
        }
    }
}