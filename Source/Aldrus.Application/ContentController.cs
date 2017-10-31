using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Aldrus.Application
{
    [Route("/api/content")]
    public class ContentController: Controller
    {
        private readonly ZonaMobiClient client;

        public ContentController(ZonaMobiClient client)
        {
            this.client = client;
        }

        [HttpGet("{id}")]
        public Task<string> Get(string id)
        {
            return client.GetContentUri(id);
        }
    }
}