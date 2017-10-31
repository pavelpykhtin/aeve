using System.Threading.Tasks;
using Policies;

namespace Aldrus.Application
{
    public class ZonaMobiClient
    {
        private readonly JsonHttpClient.JsonHttpClient client;

        public ZonaMobiClient(string baseUri, Serializer.Serializer serializer, IPolicy policy)
        {
            this.client = new JsonHttpClient.JsonHttpClient(baseUri, serializer, policy);
        }

        public async Task<string>GetContentUri(string id)
        {
            var movie = await client.GetAsync<MovieDto>($"ajax/video/{id}");

            return movie.Url;
        }
    }
}