using System;
using System.Net;
using System.Threading.Tasks;
using Aeve.Contracts;
using Policies;
using Serializer;

namespace Elbum.Application
{
    public class AeveClient : IAeveClient
    {
        private readonly JsonHttpClient.JsonHttpClient client;
        private readonly ISerializer serializer;

        public AeveClient(string baseUrl, ISerializer serializer)
            : this(baseUrl, serializer, null)
        {
            
        }

        public AeveClient(string baseUrl, ISerializer serializer, WebProxy proxy)
        {
            client = new JsonHttpClient.JsonHttpClient(baseUrl, serializer, HttpPolicies.SimpleRetry);
        }

        public Task CreateMovieAsync(CreateMovieRequest request)
        {
            Console.WriteLine($"Posting movie [{request.Id}]...");
            try
            {
                return client.PostOneWayAsync("api/movies", request);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                Console.WriteLine($"Posting movie [{request.Id}]...Done");
            }
        }
    }
}